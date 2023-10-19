using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.Events.Interactive.Contracts
{
    public abstract class RequestPayload<TRequired> : IEventDataModel
    {
        public TRequired Required { get; }

        protected RequestPayload(TRequired required)
        {
            Required = required;
        }
    }
}
