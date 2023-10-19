using System;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    public interface IEventAggregator : IPublisher, ISubscriber, IDisposable
    {
    }
}
