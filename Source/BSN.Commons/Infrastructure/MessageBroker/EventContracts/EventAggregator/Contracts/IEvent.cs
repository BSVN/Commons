using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    /// <summary>
    /// Represents an interface for events with associated data models of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of data model associated with the event.</typeparam>
    public interface IEvent<T> : IEvent where T : IEventDataModel
    {
        /// <summary>
        /// Gets the data model associated with the event.
        /// </summary>
        T DataModel { get; }
    }

    /// <summary>
    /// Represents a generic interface for events.
    /// </summary>
    public interface IEvent
    {

    }
}
