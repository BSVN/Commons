namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    public enum DeliveryMode : byte
    {
        Persistent = 2,
        NonPersistent = 1
    }
}
