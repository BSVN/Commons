using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    /// <summary>
    /// Represents an abstract base class for event objects with associated data in the Event Aggregator pattern. 
    /// This class is a fundamental building block for implementing event-driven architectures.
    /// For more information on the Event Aggregator pattern, please refer to the documentation at: 
    /// <see href="Source/BSN.Commons/Infrastructure/MessageBroker/README.md"/>.
    /// </summary>
    /// <typeparam name="T">The type of data model associated with the event. Data models provide structured information
    /// that can be associated with events and used to convey additional context or details.</typeparam>
    public abstract class EventBase<T> : IEvent<T> where T : IEventDataModel
    {
        /// <summary>
        /// Gets or sets the data model associated with the event. Data models are used to encapsulate event-specific information,
        /// making it possible to transmit complex data along with events in a structured and organized manner.
        /// </summary>
        public T DataModel { get; set; }
    }
}
