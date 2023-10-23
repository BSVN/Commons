using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    /// <summary>
    /// Represents an interface for events with associated data models of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of data model associated with the event.</typeparam>
    /// <summary>
    /// Represents an interface for events with associated data models of type <typeparamref name="T"/>. 
    /// In event-driven architectures, events provide a mechanism for loosely coupling components by 
    /// allowing them to communicate without needing to be aware of each other's existence. This interface 
    /// serves as a foundational concept for defining events with specific data models, enabling structured 
    /// and contextual information to be transmitted along with the events.
    /// </summary>
    /// <typeparam name="T">The type of data model associated with the event. Data models encapsulate relevant 
    /// information that can provide additional context or details related to the event.</typeparam>
    public interface IEvent<T> : IEvent where T : IEventDataModel
    {
        /// <summary>
        /// Gets or sets the data model associated with the event. The data model serves as a container for structured 
        /// information, allowing events to convey specific details or context. By including data models in events, 
        /// components can make use of this information for various purposes, such as processing, logging, or decision-making.
        /// </summary>
        T DataModel { get; set; }
    }

    /// <summary>
    /// Represents a generic interface for events. In event-driven systems, events play a central role in facilitating 
    /// communication and coordination among various components. This interface defines the basic contract for events 
    /// without specifying a particular data model, providing flexibility for a wide range of event types and purposes.
    /// </summary>
    public interface IEvent
    {

    }
}
