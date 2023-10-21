using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using System.Threading.Tasks;
using BSN.Commons.Infrastructure.MessageBroker.EventAggregator;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BSN.Commons.Infrastructure.MessageBroker.Kafka
{
    /// <summary>
    /// Represents an implementation of an event aggregator,
    /// a fundamental component in an event-driven architecture,
    /// responsible for aggregating events and broadcasting them to interested subscribers.
    /// this implementation uses Kafka as the underlying message broker.
    /// </summary>
    public class KafkaEventAggregator : IEventAggregator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaEventAggregator"/> class.
        /// </summary>
        /// <param name="connectionOptions">The Kafka connection options.</param>
        /// <param name="subscriptionManager">The subscription manager that manages event subscriptions.</param>
        /// <param name="logger">The logger.</param>
        public KafkaEventAggregator(
            KafkaConnectionOptions connectionOptions, 
            IEventAggregatorSubscriptionManager subscriptionManager, 
            ILogger logger
            )
        {
            _logger = logger;
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = connectionOptions.BootstrapServers
            };
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = connectionOptions.BootstrapServers,
                GroupId = connectionOptions.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _subscriptionManager = subscriptionManager ?? new InMemoryEventAggregatorSubscriptionManager();
            _consumers = new Dictionary<string, IConsumer<Null, string>>();
            _consumersCancellationTokenSources = new Dictionary<string, CancellationTokenSource>();
        }

        /// <inheritdoc />
        public async void Publish<TEventModel>(IEvent<TEventModel> @event) where TEventModel : IEventDataModel
        {
            await PublishAsync(@event);
        }

        /// <inheritdoc />
        public Task PublishAsync<TEventModel>(IEvent<TEventModel> @event) where TEventModel : IEventDataModel
        {
            // we did not do this in the constructor because we do not want to create a producer if we do not need it.
            // right now for our use case, we only need to publish events or subscribe to them.
            // we do not need both at the same time.
            if (_producer == null)
            {
                _producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
            }
            
            string eventName = @event.GetType().FullName;
            
            string messageString = JsonConvert.SerializeObject(@event);

            var message = new Message<Null, string>
            {
                Value = messageString
            };
            
            return _producer.ProduceAsync(eventName, message);
        }

        /// <inheritdoc />
        public bool HasSubscriptionForEvent<TEvent, TEventDataModel>() where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            return _subscriptionManager.HasSubscriptionsForEvent<TEvent>();
        }

        /// <inheritdoc />
        public void Subscribe<TEvent, TEventDataModel>(IEventReceiver eventReceiver) where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            try
            {
                _subscriptionManager.AddSubscription<TEvent>(eventReceiver);
                
                CreateConsumerForEvent<TEvent, TEventDataModel>(eventReceiver);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error subscribing to event {typeof(TEvent).FullName}.");
                
                _subscriptionManager.RemoveSubscription(typeof(TEvent).FullName);
                
                throw;
            }
        }

        /// <inheritdoc />
        public void UnSubscribe<TEvent, TEventDataModel>(IEventReceiver eventReceiver) where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            string eventName = typeof(TEvent).Name;
            
            if (!_consumers.ContainsKey(eventName))
            {
                _logger.LogWarning($"Consumer for event {eventName} does not exist.");
                return;
            }
            
            _consumersCancellationTokenSources[eventName].Cancel();
            
            _consumers.Remove(eventName);
            
            _consumersCancellationTokenSources.Remove(eventName);
            
            _subscriptionManager.RemoveSubscription(eventName);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _producer?.Dispose();
            
            foreach (var consumer in _consumers)
            {
                _consumersCancellationTokenSources[consumer.Key].Cancel();
                
                consumer.Value.Dispose();
            }
        }
        
        private void CreateConsumerForEvent<TEvent, TEventDataModel>(IEventReceiver eventReceiver)
            where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            string eventName = typeof(TEvent).Name;

            if (_consumers.ContainsKey(eventName))
            {
                _logger.LogWarning($"Consumer for event {eventName} already exists.");
                return;
            }

            var consumer = new ConsumerBuilder<Null, string>(_consumerConfig).Build();

            consumer.Subscribe(eventName);

            _consumers.Add(eventName, consumer);

            var cancellationTokenSource = new CancellationTokenSource();

            _consumersCancellationTokenSources.Add(eventName, cancellationTokenSource);

            Task.Run(
                () => StartConsume<TEvent, TEventDataModel>(eventReceiver, consumer, cancellationTokenSource), 
                cancellationTokenSource.Token
                );
        }

        private void StartConsume<TEvent, TEventDataModel>(IEventReceiver eventReceiver, IConsumer<Null, string> consumer, CancellationTokenSource cancellationTokenSource) where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume();
                
                if (consumeResult == null)
                {
                    continue;
                }
                
                var message = consumeResult.Message.Value;
                
                var eventModel = JsonConvert.DeserializeObject<TEvent>(message);
                
                eventReceiver.Handle(eventModel);
            }
        }
        
        private IProducer<Null, string> _producer;
        private readonly IEventAggregatorSubscriptionManager _subscriptionManager;
        private readonly Dictionary<string, IConsumer<Null, string>> _consumers;
        private readonly Dictionary<string, CancellationTokenSource> _consumersCancellationTokenSources;
        private readonly ProducerConfig _producerConfig;
        private readonly ConsumerConfig _consumerConfig;
        private readonly ILogger _logger;
    }
}
