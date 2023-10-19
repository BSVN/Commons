using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    public interface ISubscriber
    {
        bool HasSubscriptionForEvent<TEvent, TEventDataModel>() where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel;

        void Subscribe<TEvent, TEventDataModel>(IEventReceiver eventReceiver) where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel;

        void UnSubscribe<TEvent, TEventDataModel>(IEventReceiver eventReceiver) where TEvent : IEvent<TEventDataModel> where TEventDataModel : IEventDataModel;
    }
}
