using System;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    /// <summary>
    /// Represents an interface for an event aggregator, combining the fundamental functionality of a publisher, subscriber, and disposable object. 
    /// In architectural design, an event aggregator is a crucial component used to facilitate communication and coordination among various parts 
    /// of a software system. It acts as a centralized hub for the distribution of events, allowing different components to publish and subscribe to 
    /// events without having direct knowledge of each other. By merging the roles of publisher, subscriber, and IDisposable, this interface encapsulates 
    /// key responsibilities within the event-driven architecture.
    /// </summary>
    public interface IEventAggregator : IPublisher, ISubscriber, IDisposable
    {
    }
}
