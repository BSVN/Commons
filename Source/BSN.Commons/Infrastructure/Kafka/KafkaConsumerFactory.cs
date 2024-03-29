﻿using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace BSN.Commons.Infrastructure.Kafka
{
    /// <inheritdoc />
    public class KafkaConsumerFactory<T> : IKafkaConsumerFactory<T>
    {
        /// TODO: Ebrahim: Use Lazy Pattern
        /// <param name="options">Default Options for KafkaConsumers</param>
        public KafkaConsumerFactory(IKafkaConsumerOptions options)
        {
            _defaultConsumerOptions = options;
            _consumers = new Dictionary<string, KafkaConsumer<T>>();
        }

        /// <inheritdoc/>
        public IKafkaConsumer<T> Create(string topic, string groupId)
        {
            string consumerKey = topic + ":" + groupId;

            if (_consumers.ContainsKey(consumerKey))
            {
                return _consumers[consumerKey];
            }

            var config = new ConsumerConfig() 
            {
                BootstrapServers = _defaultConsumerOptions.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = groupId
            };
            
            // Here we did this because the ReceiveMessageMaxBytes in ProducerConfig type
            // is int and can not accept high values that we expect
            config.Set("receive.message.max.bytes", _defaultConsumerOptions.ReceiveMessageMaxBytes);

            // Here Null means that the key in kafka message is null
            // it helps equal distribution of messages in the kafka cluster
            var consumerEngine = new ConsumerBuilder<Null, T>(config).Build();
            consumerEngine.Subscribe(topic);

            var consumer = new KafkaConsumer<T>(consumerEngine);
                
            _consumers.Add(consumerKey, consumer);

            return consumer;
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            foreach (var consumer in _consumers)
            {
                consumer.Value.Dispose();
            }
        }
        
        private readonly Dictionary<string, KafkaConsumer<T>> _consumers;
        private readonly IKafkaConsumerOptions _defaultConsumerOptions;
    }
}