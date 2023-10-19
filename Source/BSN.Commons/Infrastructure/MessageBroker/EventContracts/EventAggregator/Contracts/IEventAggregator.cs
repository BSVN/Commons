using System;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    /// <summary>
    /// Represents an interface for an event aggregator, combining the functionality of a publisher, subscriber, and disposable object.
    /// </summary>
    public interface IEventAggregator : IPublisher, ISubscriber, IDisposable
    {
    }
}
