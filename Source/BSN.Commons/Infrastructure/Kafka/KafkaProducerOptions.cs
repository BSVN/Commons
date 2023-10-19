namespace BSN.Commons.Infrastructure.Kafka
{
    public class KafkaProducerOptions : IKafkaProducerOptions
    {
        public KafkaProducerOptions(string bootstrapServers, string receiveMessageMaxBytes)
        {
            BootstrapServers = bootstrapServers;
            ReceiveMessageMaxBytes = receiveMessageMaxBytes;
        }

        public string BootstrapServers { get; }
        public string ReceiveMessageMaxBytes { get; }
    }
}