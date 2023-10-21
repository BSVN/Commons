using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.Events.Interactive.Contracts
{
    /// <summary>
    /// Represents an abstract base class for request payloads, providing required data of type <typeparamref name="TRequired"/>.
    /// </summary>
    /// <typeparam name="TRequired">The type of required data for the request.</typeparam>
    public abstract class RequestPayload<TRequired> : IEventDataModel
    {
        /// <summary>
        /// Gets the required data for the request.
        /// </summary>
        public TRequired Required { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestPayload{TRequired}"/> class with the specified required data.
        /// </summary>
        /// <param name="required">The required data for the request.</param>
        protected RequestPayload(TRequired required)
        {
            Required = required;
        }
    }
}
