using System.Threading.Tasks;
using BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.EventModels;

namespace BSN.Commons.Infrastructure.MessageBroker.EventContracts.EventAggregator.Contracts
{
    public interface IPublisher
    {
        void Publish<TEventModel>(IEvent<TEventModel> @event) where TEventModel : IEventDataModel;

        Task PublishAsync<TEventModel>(IEvent<TEventModel> @event) where TEventModel : IEventDataModel;
    }
}
