namespace BSN.Commons.Infrastructure.Kafka
{
    public class KafkaConsumerOptions : IKafkaConsumerOptions
    {
        public KafkaConsumerOptions(string bootstrapServers, string receiveMessageMaxBytes)
        {
            BootstrapServers = bootstrapServers;
            ReceiveMessageMaxBytes = receiveMessageMaxBytes;
        }

        public string BootstrapServers { get; }
        public string ReceiveMessageMaxBytes { get; }
    }
}