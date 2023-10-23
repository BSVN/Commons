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
            
            _producer = _session.CreateProducer(_destination);
            _consumers = new Dictionary<string, IMessageConsumer>();
        }
        
        /// <inheritdoc />
        public void Publish<TEventModel>(IEvent<TEventModel> @event) where TEventModel : IEventDataModel
        {
            string serializedEvent = JsonConvert.SerializeObject(@event.DataModel);
            
            ITextMessage message = _session.CreateTextMessage(serializedEvent);
            
            _producer.Send(message);
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
        public void Subscribe<TEvent, TEventDataModel>(IEventReceiver eventReceiver) where TEvent : IEvent<TEventDataModel>, new() where TEventDataModel : IEventDataModel
        {
            string eventName = typeof(TEvent).Name;
            
            _subscriptionManager.AddSubscription<TEvent>(eventReceiver);
            
            var consumer = _session.CreateConsumer(_destination);
            
            consumer.Listener += message =>
            {
                if (message is ITextMessage textMessage)
                {
                    string serializedEvent = textMessage.Text;
                    
                    TEventDataModel eventDataModel = JsonConvert.DeserializeObject<TEventDataModel>(serializedEvent);
                    
                    TEvent @event = new TEvent
                    {
                        DataModel = eventDataModel
                    };
                    
                    eventReceiver.Handle(@event);
                }
            };
            
            _consumers.Add(eventName, consumer);
        }

        /// <inheritdoc />
        public void UnSubscribe<TEvent, TEventDataModel>(IEventReceiver eventReceiver) where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel
        {
            string eventName = typeof(TEvent).Name;
            
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
            _producer?.Dispose();

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
        private readonly IMessageProducer _producer;
        private readonly Dictionary<string, IMessageConsumer> _consumers;
    }
}