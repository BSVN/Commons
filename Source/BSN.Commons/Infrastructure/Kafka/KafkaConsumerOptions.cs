namespace BSN.Commons.Infrastructure.Kafka
{
    /// <inheritdoc />
    public class KafkaConsumerOptions : IKafkaConsumerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaConsumerOptions"/> class.
        /// </summary>
        /// <param name="bootstrapServers">List of kafka bootstrap servers seperated by ;</param>
        /// <param name="receiveMessageMaxBytes">Represents the Size that a Receiving message can have in Bytes</param>
        public KafkaConsumerOptions(string bootstrapServers, string receiveMessageMaxBytes)
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