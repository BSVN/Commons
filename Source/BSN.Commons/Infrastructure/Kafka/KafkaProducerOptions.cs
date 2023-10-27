namespace BSN.Commons.Infrastructure.Kafka
{
    /// <inheritdoc />
    public class KafkaProducerOptions : IKafkaProducerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaProducerOptions"/> class.
        /// </summary>
        /// <param name="bootstrapServers"></param>
        public KafkaProducerOptions(string bootstrapServers)
        {
            BootstrapServers = bootstrapServers;
        }

        /// <inheritdoc />
        public string BootstrapServers { get; }
    }
}