namespace BSN.Commons.Infrastructure.MessageBroker.RabbitMqEventAggregator
{
    /// <summary>
    /// Please refer to: <see href="https://medium.com/@bakhtmunir/rabbitmq-exchanges-in-c-8b3202fb3ab0"/>
    /// </summary>
    public enum BrokerType
    {
        /// <summary>
        /// Direct exchange type routes messages based on exact matching of routing keys.
        /// </summary>
        direct,
        
        /// <summary>
        /// Topic exchange type routes messages based on wildcard patterns in routing keys.
        /// </summary>
        topic,

        /// <summary>
        /// Headers exchange type routes messages based on message headers.
        /// </summary>
        headers,

        /// <summary>
        /// Fanout exchange type routes messages to all bound queues without considering routing keys.
        /// </summary>
        fanout
    }
}