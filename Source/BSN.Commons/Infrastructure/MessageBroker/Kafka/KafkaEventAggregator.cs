using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BSN.Commons.Infrastructure.Kafka;
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
            : this(
                  producerFactory: new KafkaProducerFactory<string>(new KafkaProducerOptions(
                      bootstrapServers: connectionOptions.BootstrapServers)),
                  consumerFactory: new KafkaConsumerFactory<string>(new KafkaConsumerOptions(
                      bootstrapServers: connectionOptions.BootstrapServers, receiveMessageMaxBytes: connectionOptions.ReceiveMessageMaxBytes)),
                  connectionOptions: connectionOptions,
                  subscriptionManager: subscriptionManager,
                  logger: logger)
        {}
        
        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaEventAggregator"/> class.
        /// </summary>
        /// <param name="producerFactory">the factory for creating Kafka producers.</param>
        /// <param name="consumerFactory">the factory for creating Kafka consumers.</param>
        /// <param name="connectionOptions">The Kafka connection options.</param>
        /// <param name="subscriptionManager">The subscription manager that manages event subscriptions.</param>
        /// <param name="logger">The logger.</param>
        public KafkaEventAggregator(
            IKafkaProducerFactory<string> producerFactory, 
            IKafkaConsumerFactory<string> consumerFactory,
            KafkaConnectionOptions connectionOptions,
            IEventAggregatorSubscriptionManager subscriptionManager, 
            ILogger logger
            )
        {
            _logger = logger;
            _kafkaConnectionOptions = connectionOptions;
            _subscriptionManager = subscriptionManager ?? new InMemoryEventAggregatorSubscriptionManager();
            _producerFactory = producerFactory;
            
            _consumerFactory = consumerFactory;
            _consumersCancellationTokenSources = new Dictionary<string, CancellationTokenSource>();
        }

        /// <inheritdoc />
        public void Publish<TEventModel>(IEvent<TEventModel> @event) where TEventModel : IEventDataModel
        {
            string eventName = @event.GetType().FullName;
            
            IKafkaProducer<string> producer = _producerFactory.Create(eventName);
            
            string message = JsonConvert.SerializeObject(@event);

            producer.Produce(message);
        }

        /// <inheritdoc />
        public Task PublishAsync<TEventModel>(IEvent<TEventModel> @event) where TEventModel : IEventDataModel
        {
            string eventName = @event.GetType().FullName;
            
            IKafkaProducer<string> producer = _producerFactory.Create(eventName);
            
            string message = JsonConvert.SerializeObject(@event);

            return producer.ProduceAsync(message);
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
            
            if (!_consumersCancellationTokenSources.ContainsKey(eventName))
            {
                _logger.LogWarning($"Consumer for event {eventName} does not exist.");
                return;
            }
            
            _consumersCancellationTokenSources[eventName].Cancel();
            
            _consumersCancellationTokenSources.Remove(eventName);
            
            _subscriptionManager.RemoveSubscription(eventName);
        }

        /// <inheritdoc />
        public void Dispose()
        {   
            foreach (var ct in _consumersCancellationTokenSources)
            {
                ct.Value.Cancel();
            }
            
            _producerFactory.Dispose();
            
            _consumerFactory.Dispose();
            
            _logger.LogInformation("Event aggregator disposed.");
        }

        private void CreateConsumerForEvent<TEvent, TEventDataModel>(IEventReceiver eventReceiver)
            where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            string eventName = typeof(TEvent).Name;

            if (_consumersCancellationTokenSources.ContainsKey(eventName))
            {
                _logger.LogWarning($"Consumer for event {eventName} already exists. Skipping creation.");
                return;
            }

            var consumer = _consumerFactory.Create(eventName, _kafkaConnectionOptions.ConsumerGroupId);
            
            var cancellationTokenSource = new CancellationTokenSource();

            _consumersCancellationTokenSources.Add(eventName, cancellationTokenSource);

            StartConsume<TEvent, TEventDataModel>(eventReceiver, consumer, cancellationTokenSource);
        }

        private async Task StartConsume<TEvent, TEventDataModel>(
            IEventReceiver eventReceiver, 
            IKafkaConsumer<string> consumer, 
            CancellationTokenSource cancellationTokenSource
            ) where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                var result = await consumer.ConsumeAsync(cancellationTokenSource.Token);
                
                if (result == null)
                {
                    continue;
                }
                
                var eventModel = JsonConvert.DeserializeObject<TEvent>(result);
                
                eventReceiver.Handle(eventModel);
            }
        }
        
        private readonly IKafkaProducerFactory<string> _producerFactory;
        private readonly IKafkaConsumerFactory<string> _consumerFactory;
        private readonly Dictionary<string, CancellationTokenSource> _consumersCancellationTokenSources;
        private readonly IEventAggregatorSubscriptionManager _subscriptionManager;
        private readonly KafkaConnectionOptions _kafkaConnectionOptions;
        private readonly ILogger _logger;
    }
}
