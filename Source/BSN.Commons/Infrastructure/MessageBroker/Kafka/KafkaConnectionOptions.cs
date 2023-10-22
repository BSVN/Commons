namespace BSN.Commons.Infrastructure.MessageBroker.Kafka
{
    /// <summary>
    /// Represents options for configuring a Kafka connection.
    /// </summary>
    public class KafkaConnectionOptions
    {
        /// <summary>
        /// these servers are used to bootstrap the initial connection to the Kafka cluster.
        /// it is a list of host/port pairs separated by commas.
        /// for example, "broker1:9092,broker2:9092".
        /// </summary>
        public string BootstrapServers { get; set; }
        
        /// <summary>
        /// this is a unique string that identifies the consumer group this consumer belongs to.
        /// each message sent to a topic is delivered to one consumer instance within each subscribing consumer group.
        /// it is useful for parallelism, fault tolerance, and scalability.
        /// </summary>
        public string ConsumerGroupId { get; set; }

        /// <summary>
        /// the maximum number of bytes in a message batch.
        /// </summary>
        public string ReceiveMessageMaxBytes { get; set; }
    }
}