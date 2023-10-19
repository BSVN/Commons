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
        
        string ReceiveMessageMaxBytes { get; }
    }
}