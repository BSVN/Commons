using System.Threading.Tasks;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    /// <summary>
    /// Represents an interface for an event receiver, a critical component in an event-driven architecture responsible for handling events with associated data models.
    /// Event receivers play a pivotal role in an event-driven system, as they are responsible for processing and responding to events, which often carry valuable data that needs to be acted upon.
    /// </summary>
    public interface IEventReceiver
    {
        /// <summary>
        /// Handles an event with an associated data model of type <typeparamref name="T"/>.
        /// This method is invoked when an event is received, allowing the receiver to process the event and the associated data model.
        /// </summary>
        /// <typeparam name="T">The type of data model associated with the event.</typeparam>
        /// <param name="event">The event to handle, which carries the associated data model.</param>
        /// <returns>A task representing the asynchronous handling of the event, which is crucial for non-blocking and responsive event processing.</returns>
        Task Handle<T>(IEvent<T> @event) where T : IEventDataModel;
    }
}
