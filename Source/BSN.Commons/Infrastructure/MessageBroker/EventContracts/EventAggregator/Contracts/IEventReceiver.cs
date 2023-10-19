using System.Threading.Tasks;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    /// <summary>
    /// Represents an interface for an event receiver, responsible for handling events with associated data models.
    /// </summary>
    public interface IEventReceiver
    {
        /// <summary>
        /// Handles an event with an associated data model of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of data model associated with the event.</typeparam>
        /// <param name="event">The event to handle.</param>
        /// <returns>A task representing the asynchronous handling of the event.</returns>
        Task Handle<T>(IEvent<T> @event) where T : IEventDataModel;
    }
}
