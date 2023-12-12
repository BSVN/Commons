using System.Collections.Generic;
using System.Threading.Tasks;
using BSN.Commons.Infrastructure.MessageBroker.EventAggregator;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.Util;
using Newtonsoft.Json;

namespace BSN.Commons.Infrastructure.MessageBroker.Jms
{
    /// <summary>
    /// Represents an implementation of <see cref="IEventAggregator"/>.
    /// using JMS as the underlying message broker.
    /// </summary>
    public class JmsEventAggregator : IEventAggregator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JmsEventAggregator"/> class.
        /// </summary>
        public JmsEventAggregator(
            JmsConnectionOptions jmsConnectionOptions, 
            IEventAggregatorSubscriptionManager eventAggregatorSubscriptionManager
            )
        {
            _subscriptionManager = eventAggregatorSubscriptionManager ?? new InMemoryEventAggregatorSubscriptionManager();

            IConnectionFactory factory = new ConnectionFactory(jmsConnectionOptions.BrokerUri);
            _connection = factory.CreateConnection();
            _connection.Start();
            _session = _connection.CreateSession();
            _destination = SessionUtil.GetDestination(_session, jmsConnectionOptions.QueueName);
            
            _producers = new Dictionary<string, IMessageProducer>();
            _consumers = new Dictionary<string, IMessageConsumer>();
        }
        
        /// <inheritdoc />
        public void Publish<TEventModel>(IEvent<TEventModel> @event) where TEventModel : IEventDataModel
        {
            string eventName = @event.GetType().FullName;
            
            if (!_producers.ContainsKey(eventName))
            {
                _producers.Add(eventName, _session.CreateProducer(_destination));
            }
            
            var producer = _producers[eventName];
            
            string serializedEvent = JsonConvert.SerializeObject(@event);

            ITextMessage message = _session.CreateTextMessage(serializedEvent);
            
            producer.Send(message);
        }

        /// <inheritdoc />
        public Task PublishAsync<TEventModel>(IEvent<TEventModel> @event) where TEventModel : IEventDataModel
        {
            return Task.Run(() => Publish(@event));
        }

        /// <inheritdoc />
        public bool HasSubscriptionForEvent<TEvent, TEventDataModel>() where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            return _subscriptionManager.HasSubscriptionsForEvent<TEvent>();
        }

        /// <inheritdoc />
        public void Subscribe<TEvent, TEventDataModel>(IEventReceiver eventReceiver) where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            string eventName = typeof(TEvent).FullName;
            
            _subscriptionManager.AddSubscription<TEvent>(eventReceiver);
            
            if (!_consumers.ContainsKey(eventName))
            {
                _consumers.Add(eventName, _session.CreateConsumer(_destination));
            }
            
            var consumer = _consumers[eventName];
            
            consumer.Listener += message =>
            {
                if (message is ITextMessage textMessage)
                {
                    string serializedEvent = textMessage.Text;
                    
                    TEvent @event = JsonConvert.DeserializeObject<TEvent>(serializedEvent);
                    
                    eventReceiver.Handle(@event);
                }
            };
        }

        /// <inheritdoc />
        public void UnSubscribe<TEvent, TEventDataModel>(IEventReceiver eventReceiver) where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            string eventName = typeof(TEvent).FullName;
            
            _subscriptionManager.RemoveSubscription(eventName);
            
            if (_consumers.ContainsKey(eventName))
            {
                _consumers[eventName].Dispose();
                _consumers.Remove(eventName);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var producer in _producers)
            {
                producer.Value.Dispose();
            }
            
            foreach (var consumer in _consumers)
            {
                consumer.Value.Dispose();
            }
            
            _destination?.Dispose();
            _session?.Dispose();
            _connection?.Dispose();
        }

        private readonly IEventAggregatorSubscriptionManager _subscriptionManager;
        private readonly IConnection _connection;
        private readonly ISession _session;
        private readonly IDestination _destination;
        private readonly Dictionary<string, IMessageConsumer> _consumers;
        private readonly Dictionary<string, IMessageProducer> _producers;
    }
}