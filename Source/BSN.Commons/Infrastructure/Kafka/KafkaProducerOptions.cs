namespace BSN.Commons.Infrastructure.Kafka
{
    /// <inheritdoc />
    public class KafkaProducerOptions : IKafkaProducerOptions
    {
        /// <param name="bootstrapServers"><see cref="BootstrapServers"/></param>
        /// <param name="receiveMessageMaxBytes"><see cref="ReceiveMessageMaxBytes"/></param>
        public KafkaProducerOptions(string bootstrapServers, string receiveMessageMaxBytes)
        {
            BootstrapServers = bootstrapServers;
            ReceiveMessageMaxBytes = receiveMessageMaxBytes;
        }

        /// <inheritdoc />
        public string BootstrapServers { get; }

        /// <inheritdoc />
        public string ReceiveMessageMaxBytes { get; }
    }
}