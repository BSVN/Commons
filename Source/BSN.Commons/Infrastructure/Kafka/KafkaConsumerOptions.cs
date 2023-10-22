namespace BSN.Commons.Infrastructure.Kafka
{
    /// <inheritdoc />
    public class KafkaConsumerOptions : IKafkaConsumerOptions
    {
        /// <inheritdoc />
        public string BootstrapServers { get; set; }

        /// <inheritdoc />
        public string ReceiveMessageMaxBytes { get; set; }
    }
}