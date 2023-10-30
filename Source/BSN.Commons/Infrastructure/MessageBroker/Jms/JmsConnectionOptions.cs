namespace BSN.Commons.Infrastructure.MessageBroker.Jms
{
    /// <summary>
    /// Represents a class containing options for configuring a JMS connection.
    /// </summary>
    public class JmsConnectionOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JmsConnectionOptions"/> class.
        /// </summary>
        /// <param name="brokerUri"></param>
        /// <param name="queueName"></param>
        public JmsConnectionOptions(string brokerUri, string queueName)
        {
            BrokerUri = brokerUri;
            QueueName = queueName;
        }

        /// <summary>
        /// For example: tcp://localhost:61616
        /// </summary>
        public string BrokerUri { get; }
        
        /// <summary>
        /// For example: testQueue
        /// </summary>
        public string QueueName { get; }
    }
}