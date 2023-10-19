using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    public interface IEvent<T> : IEvent where T : IEventDataModel
    {
        T DataModel { get; }
    }

    public interface IEvent
    {

    }
}
