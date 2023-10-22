namespace BSN.Commons.Infrastructure.Kafka
{
    /// <inheritdoc />
    public class KafkaProducerOptions : IKafkaProducerOptions
    {
        /// <inheritdoc />
        public string BootstrapServers { get; set; }
    }
}