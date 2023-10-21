using System;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator
{
    /// <summary>
    /// Represents an interface for managing event subscriptions in an event aggregator.
    /// </summary>
    public interface IEventAggregatorSubscriptionManager
    {
        /// <summary>
        /// Indicates whether the subscription manager is empty.
        /// </summary>
        /// <returns><c>true</c> if the subscription manager is empty; otherwise, <c>false</c>.</returns>
        bool IsEmpty { get; }

        /// <summary>
        /// Adds a subscription for events of type <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">The type of event to subscribe to.</typeparam>
        /// <param name="eventReceiver">The event receiver to register for the event.</param>
        void AddSubscription<TEvent>(IEventReceiver eventReceiver) where TEvent : IEvent;
        
        /// <summary>
        /// Removes a subscription for an event with the specified name.
        /// </summary>
        /// <param name="eventName">The name of the event to unsubscribe from.</param>
        void RemoveSubscription(string eventName);

        /// <summary>
        /// Checks if there are subscriptions for events of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of event to check for subscriptions.</typeparam>
        /// <returns><c>true</c> if there are subscriptions for the event type; otherwise, <c>false</c>.</returns>
        bool HasSubscriptionsForEvent<T>() where T : IEvent;

        /// <summary>
        /// Checks if there are subscriptions for an event with the specified name.
        /// </summary>
        /// <param name="eventName">The name of the event to check for subscriptions.</param>
        /// <returns><c>true</c> if there are subscriptions for the event; otherwise, <c>false</c>.</returns>
        bool HasSubscriptionsForEvent(string eventName);

        /// <summary>
        /// Retrieves the type of the event with the specified name.
        /// </summary>
        /// <param name="eventName">The name of the event type to retrieve.</param>
        /// <returns>The event type corresponding to the provided name.</returns>
        Type GetEventTypeByName(string eventName);

        /// <summary>
        /// Clears all event subscriptions managed by the subscription manager.
        /// </summary>
        void Clear();

        /// <summary>
        /// Retrieves the event receiver for events of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of event to retrieve the event receiver for.</typeparam>
        /// <returns>The event receiver for the specified event type.</returns>
        IEventReceiver GetEventReceiverForEvent<T>() where T : IEvent;

        /// <summary>
        /// Retrieves the event receiver for an event with the specified name.
        /// </summary>
        /// <param name="eventName">The name of the event to retrieve the event receiver for.</param>
        /// <returns>The event receiver for the specified event name.</returns>
        IEventReceiver GetEventReceiverForEvent(string eventName);
        
        /// <summary>
        /// Gets the unique key for an event of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of event to get the key for.</typeparam>
        /// <returns>The unique key for the event type.</returns>
        string GetEventKey<T>();
    }
}
