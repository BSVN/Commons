namespace BSN.Commons.Infrastructure.MessageBroker.Jms
{
    /// <summary>
    /// Represents a class containing options for configuring a JMS connection.
    /// </summary>
    public class JmsConnectionOptions
    {
        /// <summary>
        /// For example: tcp://localhost:61616
        /// </summary>
        public string BrokerUri { get; set; }
        
        /// <summary>
        /// For example: testQueue
        /// </summary>
        public string QueueName { get; set; }
    }
}