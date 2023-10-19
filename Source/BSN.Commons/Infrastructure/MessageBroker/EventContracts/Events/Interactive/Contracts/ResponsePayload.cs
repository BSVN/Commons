using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.Events.Interactive.Contracts
{
    /// <summary>
    /// Represents an abstract base class for response payloads, providing required data of type <typeparamref name="TRequired"/>.
    /// </summary>
    /// <typeparam name="TRequired">The type of required data for the response.</typeparam>
    public abstract class ResponsePayload<TRequired> : IEventDataModel
    {
        /// <summary>
        /// Gets the required data for the response.
        /// </summary>
        public TRequired Required { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsePayload{TRequired}"/> class with the specified required data.
        /// </summary>
        /// <param name="required">The required data for the response.</param>
        protected ResponsePayload(TRequired required)
        {
            Required = required;
        }
    }
}
