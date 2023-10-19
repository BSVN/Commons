using System;
using RabbitMQ.Client;

namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    public interface IRabbitMqConnection : IDisposable
    {
        bool IsConnected { get; }

        string UserName { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
