namespace BSN.Commons.Infrastructure.Kafka
{
    /// <summary>
    /// Represents a factory class for IKafkaProducer classes
    /// </summary>
    /// <typeparam name="T">the type which the created producer classes are based on</typeparam>
    public interface IKafkaProducerFactory<T>
    {
        /// <summary>
        /// Creates a IKafkaProducer
        /// </summary>
        /// <param name="topic">Represents the topic which the producer will produce on,
        /// Read https://dattell.com/data-architecture-blog/what-is-a-kafka-topic/ for further info.</param>
        /// <returns>An IKafkaProducer instance</returns>
        IKafkaProducer<T> Create(string topic);
    }
}