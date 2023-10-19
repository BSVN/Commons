using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    /// <summary>
    /// Represents an abstract base class for event objects with associated data. refer to: <see href="https://tfs.resaa.net/Resaa/Resaa/_wiki/wikis/Resaa.wiki/48/Event-Aggregator"/>
    /// </summary>
    /// <typeparam name="T">The type of data model associated with the event.</typeparam>
    public abstract class EventBase<T> : IEvent<T> where T : IEventDataModel
    {
        /// <summary>
        /// Gets or sets the data model associated with the event.
        /// </summary>
        public T DataModel { get; set; }
    }
}
