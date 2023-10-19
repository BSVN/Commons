using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.Events.Interactive.Contracts
{
    public abstract class ResponsePayload<TRequired> : IEventDataModel
    {
        public TRequired Required { get; }

        protected ResponsePayload(TRequired required)
        {
            Required = required;
        }
    }
}
