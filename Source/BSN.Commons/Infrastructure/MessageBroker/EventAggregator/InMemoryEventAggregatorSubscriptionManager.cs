using System;
using System.Collections.Generic;
using System.Linq;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts;
using IEvent = BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts.IEvent;

namespace BSN.Commons.Infrastructure.MessageBroker.EventAggregator
{
    public partial class InMemoryEventAggregatorSubscriptionManager : IEventAggregatorSubscriptionManager
    {
        public InMemoryEventAggregatorSubscriptionManager()
        {
            _eventReceivers = new Dictionary<string, IEventReceiver>();
            _eventsType = new List<Type>();
        }

        public void AddSubscription<TEvent>(IEventReceiver eventReceiver) where TEvent : IEvent
        {
            string eventName = GetEventKey<TEvent>();

            DoAddSubscription(eventReceiver, eventName);

            if (!_eventsType.Contains(typeof(TEvent)))
            {
                _eventsType.Add(typeof(TEvent));
            }
        }

        private void DoAddSubscription(IEventReceiver eventReceiver, string eventName)
        {
            if (_eventReceivers.ContainsKey(eventName))
            {
                throw new ArgumentException(
                    $"Receiver {nameof(eventReceiver)} already registered for '{eventName}'", nameof(eventReceiver));
            }

            _eventReceivers[eventName] = eventReceiver;
        }

        public void RemoveSubscription(string eventName)
        {
            RemoveEventReceiver(eventName);
        }

        public IEventReceiver GetEventReceiverForEvent<T>() where T : IEvent
        {
            string key = GetEventKey<T>();

            return _eventReceivers[key];
        }

        public IEventReceiver GetEventReceiverForEvent(string eventName)
        {
            return _eventReceivers[eventName];
        }

        public string GetEventKey<T>()
        {
            return typeof(T).FullName;
        }

        public bool HasSubscriptionsForEvent<T>() where T : IEvent
        {
            string key = GetEventKey<T>();

            return HasSubscriptionsForEvent(key);
        }

        public bool HasSubscriptionsForEvent(string eventName)
        {
            return _eventReceivers.ContainsKey(eventName);
        }

        public void Clear() => _eventReceivers.Clear();

        public Type GetEventTypeByName(string eventName) => _eventsType.SingleOrDefault(t => t.FullName == eventName);

        private void RemoveEventReceiver(string eventName)
        {
            if (_eventReceivers.ContainsKey(eventName))
            {
                _eventReceivers.Remove(eventName);

                Type eventType = _eventsType.SingleOrDefault(e => e.FullName == eventName);

                if (eventType != null)
                {
                    _eventsType.Remove(eventType);
                }
            }
        }

        private IEventReceiver FindEventReceiver(string eventName)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                return null;
            }

            return _eventReceivers[eventName];
        }

        #region local variables

        private readonly Dictionary<string, IEventReceiver> _eventReceivers;

        private readonly List<Type> _eventsType;

        public bool IsEmpty => !_eventReceivers.Keys.Any();

        #endregion 

    }
}
