using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BSN.Commons.Infrastructure.MessageBroker.EventAggregator;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    /// <summary>
    /// Provides event aggregation and message distribution functionality using RabbitMQ as the message broker.
    /// </summary>
    public class RabbitMqEventAggregator : IEventAggregator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqEventAggregator"/> class.
        /// </summary>
        /// <param name="connection">The RabbitMQ connection to use.</param>
        /// <param name="eventAggregatorSubscriptionsManager">The event aggregator subscription manager.</param>
        /// <param name="rabbitMqOptions">The RabbitMQ options for configuration.</param>
        /// <param name="logger">The logger for capturing log events.</param>
        public RabbitMqEventAggregator(IRabbitMqConnection connection, IEventAggregatorSubscriptionManager eventAggregatorSubscriptionsManager,
            IOptions<RabbitMqOptions> rabbitMqOptions, ILogger logger)
        {
            _logger = logger;
            _connection = connection;
            _subscriptionsManager = eventAggregatorSubscriptionsManager ?? new InMemoryEventAggregatorSubscriptionManager();
            _options = rabbitMqOptions;
            _eventReceiverCallers = new Dictionary<string, Action<object, IEventReceiver>>();

            _consumerChannel = CreateConsumerChannel();
            _consumeTag = StartBasicConsume();
        }

        /// <inheritdoc />
        public bool HasSubscriptionForEvent<TEvent, TEventDataModel>() where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            return _subscriptionsManager.HasSubscriptionsForEvent<TEvent>();
        }

        /// <inheritdoc />
        public void Subscribe<TEvent, TEventDataModel>(IEventReceiver eventReceiver) where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            string eventName = _subscriptionsManager.GetEventKey<TEvent>();

            try
            {
                _subscriptionsManager.AddSubscription<TEvent>(eventReceiver);

                _eventReceiverCallers.Add(eventName, (@object, receiver) =>
                {
                    receiver.Handle((TEvent)@object);
                });

                DoSubscribe(eventName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                _subscriptionsManager.RemoveSubscription(eventName);

                throw ex;
            }
        }

        /// <inheritdoc />
        public void UnSubscribe<TEvent, TEventDataModel>(IEventReceiver eventReceiver) where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            string eventName = _subscriptionsManager.GetEventKey<TEvent>();

            _logger.LogTrace("UnSubscribe RabbitMQ Subscriber: EventName : {EventName}, EventReceiver : {EventReceiver}", eventName, nameof(eventReceiver));

            _subscriptionsManager.RemoveSubscription(eventName);

            _eventReceiverCallers.Remove(eventName);

            DoUnSubscribe(eventName);
        }

        /// <inheritdoc />
        public void Publish<TModel>(IEvent<TModel> @event) where TModel : IEventDataModel
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>().Or<SocketException>()
                .WaitAndRetry(_options.Value.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Could not publish event: {EventName} after {Timeout}s ({ExceptionMessage})", nameof(@event), $"{time.TotalSeconds:n1}", ex.Message);
                });

            if (@event.DataModel == null)
                throw new ArgumentNullException("DataModel", "EventDataModel is null");

            string eventName = @event.GetType().FullName;

            using (var channel = _connection.CreateModel())
            {
                ExchangeOptions exchangeOptions = _options.Value.ExchangeOptions;

                channel.ExchangeDeclare(exchangeOptions.BrokerName, exchangeOptions.ExchangeType.ToString().ToLower(), exchangeOptions.Durable,
                    exchangeOptions.AutoDelete, exchangeOptions.Arguments);

                string message = JsonConvert.SerializeObject(@event);

                byte[] body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    IBasicProperties properties = channel.CreateBasicProperties();

                    properties.DeliveryMode = (byte)_options.Value.ConsumeOptions.DeliveryMode;

                    _logger.LogTrace($"Publishing event to RabbitMQ: {eventName}");

                    channel.BasicPublish(
                        exchange: exchangeOptions.BrokerName,
                        routingKey: eventName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                });
            }
        }

        /// <inheritdoc />
        public async Task PublishAsync<TModel>(IEvent<TModel> @event) where TModel : IEventDataModel
        {
            await Task.Run(() => Publish<TModel>(@event));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            _subscriptionsManager.Clear();
        }

        private string StartBasicConsume()
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                QueueOptions queueOptions = _options.Value.QueueOptions;

                _consumerChannel.QueueDeclare(queue: queueOptions.DefaultQueueName, durable: queueOptions.Durable,
                    exclusive: queueOptions.Exclusive, autoDelete: queueOptions.AutoDelete);

                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += ConsumerReceived;

                string tag = _consumerChannel.BasicConsume(queue: queueOptions.DefaultQueueName,
                    autoAck: _options.Value.ConsumeOptions.AutoAck, consumer: consumer);

                return tag;
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");

                return null;
            }
        }

        private async Task ConsumerReceived(object sender, BasicDeliverEventArgs eventArgs)
        {
            string eventName = eventArgs.RoutingKey;

            string message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }

                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }

            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private IModel CreateConsumerChannel()
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            IModel channel = _connection.CreateModel();

            ExchangeOptions exchangeOptions = _options.Value.ExchangeOptions;
            QueueOptions queueOptions = _options.Value.QueueOptions;

            channel.ExchangeDeclare(exchangeOptions.BrokerName, exchangeOptions.ExchangeType.ToString(), exchangeOptions.Durable,
                    exchangeOptions.AutoDelete, exchangeOptions.Arguments);

            channel.QueueDeclare(queue: _connection.UserName, durable: queueOptions.Durable, exclusive: queueOptions.Exclusive,
                autoDelete: false, arguments: queueOptions.Arguments);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();

                StartBasicConsume();
            };

            return channel;
        }

        private void DoSubscribe(string eventName)
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            if (_consumerChannel.IsClosed)
            {
                _consumerChannel = CreateConsumerChannel();
                _consumeTag = StartBasicConsume();
            }

            using (IModel channel = _connection.CreateModel())
            {
                channel.QueueBind(queue: _options.Value.QueueOptions.DefaultQueueName,
                                  exchange: _options.Value.ExchangeOptions.BrokerName,
                                  routingKey: eventName);
            }
        }

        private void DoUnSubscribe(string eventName)
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            using (var channel = _connection.CreateModel())
            {
                channel.QueueUnbind(queue: _options.Value.QueueOptions.DefaultQueueName,
                    exchange: _options.Value.ExchangeOptions.BrokerName,
                    routingKey: eventName);

                if (_subscriptionsManager.IsEmpty)
                {
                    _consumerChannel.BasicCancelNoWait(_consumeTag);
                    _consumerChannel.Close();
                }
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_subscriptionsManager.HasSubscriptionsForEvent(eventName))
            {
                IEventReceiver eventReceiver = _subscriptionsManager.GetEventReceiverForEvent(eventName);

                if (eventReceiver != null)
                {
                    Type eventType = _subscriptionsManager.GetEventTypeByName(eventName);

                    object @event = JsonConvert.DeserializeObject(message, eventType);

                    var caller = _eventReceiverCallers[eventType.FullName];

                    await Task.Yield();

                    caller(@event, eventReceiver);
                }
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }

        private readonly Dictionary<string, Action<object, IEventReceiver>> _eventReceiverCallers;
        private readonly IRabbitMqConnection _connection;
        private readonly IOptions<RabbitMqOptions> _options;
        private IModel _consumerChannel;
        private string _consumeTag;
        private readonly IEventAggregatorSubscriptionManager _subscriptionsManager;
        private readonly ILogger _logger;
    }
}