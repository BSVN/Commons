using System.Threading.Tasks;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    public interface IEventReceiver
    {
        Task Handle<T>(IEvent<T> @event) where T : IEventDataModel;
    }
}
