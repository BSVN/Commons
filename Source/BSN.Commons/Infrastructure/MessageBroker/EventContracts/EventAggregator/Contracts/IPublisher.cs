using System.Threading.Tasks;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    /// <summary>
    /// Represents an interface for an event publisher, responsible for publishing events. />.
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// Publishes an event of type <typeparamref name="TEventModel"/>.
        /// </summary>
        /// <typeparam name="TEventModel">The type of event model to publish.</typeparam>
        /// <param name="event">The event to publish.</param>
        void Publish<TEventModel>(IEvent<TEventModel> @event) where TEventModel : IEventDataModel;

        /// <summary>
        /// Asynchronously publishes an event of type <typeparamref name="TEventModel"/>.
        /// </summary>
        /// <typeparam name="TEventModel">The type of event model to publish.</typeparam>
        /// <param name="event">The event to publish.</param>
        /// <returns>A task representing the asynchronous publishing of the event.</returns>
        Task PublishAsync<TEventModel>(IEvent<TEventModel> @event) where TEventModel : IEventDataModel;
    }
}
