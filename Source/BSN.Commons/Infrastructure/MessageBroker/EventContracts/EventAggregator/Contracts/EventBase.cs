using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    public abstract class EventBase<T> : IEvent<T> where T : IEventDataModel
    {
        public T DataModel { get; set; }
    }
}
