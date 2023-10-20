using System.Threading.Tasks;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    /// <summary>
    /// Represents an interface for an event publisher, a fundamental component in an event-driven architecture, responsible for publishing events.
    /// In an event-driven system, publishers play a central role in broadcasting events to interested subscribers, enabling efficient communication and decoupling between system components.
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// Publishes an event of type <typeparamref name="TEventModel"/>.
        /// This method is responsible for broadcasting an event to all interested subscribers within the system.
        /// </summary>
        /// <typeparam name="TEventModel">The type of event model to publish.</typeparam>
        /// <param name="event">The event to publish, carrying the data model associated with the event.</param>
        void Publish<TEventModel>(IEvent<TEventModel> @event) where TEventModel : IEventDataModel;

        /// <summary>
        /// Asynchronously publishes an event of type <typeparamref name="TEventModel"/>.
        /// Asynchronous event publishing allows for responsive communication within the system without blocking the publisher's operations.
        /// </summary>
        /// <typeparam name="TEventModel">The type of event model to publish.</typeparam>
        /// <param name="event">The event to publish, carrying the data model associated with the event.</param>
        /// <returns>A task representing the asynchronous publishing of the event, ensuring non-blocking event propagation and system responsiveness.</returns>
        Task PublishAsync<TEventModel>(IEvent<TEventModel> @event) where TEventModel : IEventDataModel;
    }
}
