namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels
{
    /// <summary>
    /// Represents an interface for an event data model, a fundamental concept in event-driven architectures. An event data model defines the structured data that is associated with an event, providing the necessary payload to convey meaningful information during event processing.
    /// </summary>
    /// <remarks>
    /// Event data models serve as a structured container for relevant data, allowing events to carry contextual information. These data models enable the seamless exchange of information between event publishers and subscribers, facilitating communication and coordination within a distributed system. By adhering to the defined structure of the event data model, event-driven components can interpret and process event payloads accurately.
    /// </remarks>
    public interface IEventDataModel
    {
    }
}
