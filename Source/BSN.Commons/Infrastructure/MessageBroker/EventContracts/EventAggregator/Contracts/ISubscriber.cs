using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    /// <summary>
    /// Represents an interface for an event subscriber, responsible for managing event subscriptions.
    /// </summary>
    public interface ISubscriber
    {
        /// <summary>
        /// Checks if a subscription exists for a specific event type with associated data model.
        /// </summary>
        /// <typeparam name="TEvent">The type of event to check for a subscription.</typeparam>
        /// <typeparam name="TEventDataModel">The type of data model associated with the event.</typeparam>
        /// <returns><c>true</c> if a subscription exists; otherwise, <c>false</c>.</returns>
        bool HasSubscriptionForEvent<TEvent, TEventDataModel>() where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel;

        /// <summary>
        /// Subscribes an event receiver to handle events of a specific type with associated data model.
        /// </summary>
        /// <typeparam name="TEvent">The type of event to subscribe to.</typeparam>
        /// <typeparam name="TEventDataModel">The type of data model associated with the event.</typeparam>
        /// <param name="eventReceiver">The event receiver to subscribe.</param>
        void Subscribe<TEvent, TEventDataModel>(IEventReceiver eventReceiver) where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel;

        /// <summary>
        /// Unsubscribes an event receiver from handling events of a specific type with associated data model.
        /// </summary>
        /// <typeparam name="TEvent">The type of event to unsubscribe from.</typeparam>
        /// <typeparam name="TEventDataModel">The type of data model associated with the event.</typeparam>
        /// <param name="eventReceiver">The event receiver to unsubscribe.</param>
        void UnSubscribe<TEvent, TEventDataModel>(IEventReceiver eventReceiver) where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel;
    }
}
