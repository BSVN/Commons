using System;
using System.Collections.Generic;
using System.Linq;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts;
using IEvent = BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts.IEvent;

namespace BSN.Commons.Infrastructure.MessageBroker.EventAggregator
{
    /// <summary>
    /// Manages event subscriptions for the in-memory event aggregator.
    /// </summary>
    public partial class InMemoryEventAggregatorSubscriptionManager : IEventAggregatorSubscriptionManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryEventAggregatorSubscriptionManager"/> class.
        /// </summary>
        public InMemoryEventAggregatorSubscriptionManager()
        {
            _eventReceivers = new Dictionary<string, IEventReceiver>();
            _eventsType = new List<Type>();
        }

        /// <summary>
        /// Adds a subscription for a specific event type.
        /// </summary>
        /// <typeparam name="TEvent">The type of event to subscribe to.</typeparam>
        /// <param name="eventReceiver">The event receiver to register.</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddSubscription<TEvent>(IEventReceiver eventReceiver) where TEvent : IEvent
        {
            string eventName = GetEventKey<TEvent>();

            RegisterEventReceiver(eventReceiver, eventName);

            if (!_eventsType.Contains(typeof(TEvent)))
            {
                _eventsType.Add(typeof(TEvent));
            }
        }

        /// <summary>
        /// Removes a subscription for a specific event by name.
        /// </summary>
        /// <param name="eventName">The name of the event to unsubscribe from.</param>
        /// <exception cref="ArgumentException"></exception>
        public void RemoveSubscription(string eventName)
        {
            RemoveEventReceiver(eventName);
        }

        /// <summary>
        /// Retrieves the event receiver for a specific event type.
        /// </summary>
        /// <typeparam name="T">The type of event to retrieve the event receiver for.</typeparam>
        /// <returns>The event receiver for the specified event type.</returns>
        public IEventReceiver GetEventReceiverForEvent<T>() where T : IEvent
        {
            string key = GetEventKey<T>();

            return _eventReceivers[key];
        }

        /// <summary>
        /// Retrieves the event receiver for a specific event by name.
        /// </summary>
        /// <param name="eventName">The name of the event to retrieve the event receiver for.</param>
        /// <returns>The event receiver for the specified event name.</returns>
        /// <exception cref="ArgumentException"></exception>
        public IEventReceiver GetEventReceiverForEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentException(
                    $"Event name can not be null or empty.");
            }

            return _eventReceivers[eventName];
        }

        /// <summary>
        /// Gets the unique key for an event type.
        /// </summary>
        /// <typeparam name="T">The type of event to get the key for.</typeparam>
        /// <returns>The unique key for the event type.</returns>
        public string GetEventKey<T>()
        {
            return typeof(T).FullName;
        }

        /// <summary>
        /// Checks if there are subscriptions for a specific event type.
        /// </summary>
        /// <typeparam name="T">The type of event to check for subscriptions.</typeparam>
        /// <returns>True if there are subscriptions for the event type; otherwise, false.</returns>
        public bool HasSubscriptionsForEvent<T>() where T : IEvent
        {
            string key = GetEventKey<T>();

            return HasSubscriptionsForEvent(key);
        }

        /// <summary>
        /// Checks if there are subscriptions for a specific event by name.
        /// </summary>
        /// <param name="eventName">The name of the event to check for subscriptions.</param>
        /// <returns>True if there are subscriptions for the event; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool HasSubscriptionsForEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentException(
                    $"Event name can not be null or empty.");
            }

            return _eventReceivers.ContainsKey(eventName);
        }

        /// <summary>
        /// Clears all event subscriptions.
        /// </summary>
        public void Clear() => _eventReceivers.Clear();

        /// <summary>
        /// Retrieves the event type by name.
        /// </summary>
        /// <param name="eventName">The name of the event type to retrieve.</param>
        /// <returns>The event type corresponding to the provided name.</returns> 
        /// <exception cref="ArgumentException"></exception>
        public Type GetEventTypeByName(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentException(
                    $"Event name can not be null or empty.");
            }

            return _eventsType.SingleOrDefault(t => t.FullName == eventName);
        }

        public bool IsEmpty => !_eventReceivers.Keys.Any();

        private void RegisterEventReceiver(IEventReceiver eventReceiver, string eventName)
        {
            if (_eventReceivers.ContainsKey(eventName))
            {
                throw new ArgumentException(
                    $"Receiver {nameof(eventReceiver)} already registered for '{eventName}'", nameof(eventReceiver));
            }

            _eventReceivers[eventName] = eventReceiver;
        }

        private void RemoveEventReceiver(string eventReceiverName)
        {
            if (string.IsNullOrEmpty(eventReceiverName))
            {
                throw new ArgumentException(
                    $"Event receiver name can not be null or empty.");
            }

            if (_eventReceivers.ContainsKey(eventReceiverName))
            {
                _eventReceivers.Remove(eventReceiverName);

                Type eventType = _eventsType.SingleOrDefault(e => e.FullName == eventReceiverName);

                if (eventType != null)
                {
                    _eventsType.Remove(eventType);
                }
            }
            else
            {
                throw new ArgumentException(
                    $"Event Receiver \"{eventReceiverName}\" does not exist.");
            }
        }

        private IEventReceiver FindEventReceiver(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentException(
                    $"Event name can not be null or empty.");
            }

            if (!HasSubscriptionsForEvent(eventName))
            {
                return null;
            }

            return _eventReceivers[eventName];
        }

        private readonly Dictionary<string, IEventReceiver> _eventReceivers;

        private readonly List<Type> _eventsType;
        
    }
}
