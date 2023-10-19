using System;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator
{
    public interface IEventAggregatorSubscriptionManager
    {
        bool IsEmpty { get; }

        void AddSubscription<TEvent>(IEventReceiver eventReceiver) where TEvent : IEvent;

        void RemoveSubscription(string eventName);

        bool HasSubscriptionsForEvent<T>() where T : IEvent;

        bool HasSubscriptionsForEvent(string eventName);

        Type GetEventTypeByName(string eventName);

        void Clear();

        IEventReceiver GetEventReceiverForEvent<T>() where T : IEvent;

        IEventReceiver GetEventReceiverForEvent(string eventName);

        string GetEventKey<T>();
    }
}
