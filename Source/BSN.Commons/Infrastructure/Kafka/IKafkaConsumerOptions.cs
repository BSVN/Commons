namespace BSN.Commons.Infrastructure.Kafka
{
    /// <summary>
    /// Represents the Options that are necessary for creating a kafka consumer 
    /// </summary>
    public interface IKafkaConsumerOptions
    {
        /// <summary>
        /// List of kafka bootstrap servers seperated by ;
        /// </summary>
        string BootstrapServers { get; }
        
        /// <summary>
        /// Represents the Size that a Receiving message can have in Bytes
        /// </summary>
        string ReceiveMessageMaxBytes { get; }
    }
}