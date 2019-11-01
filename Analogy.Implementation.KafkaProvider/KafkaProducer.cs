using System;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Confluent.Kafka;

namespace Analogy.Implementation.KafkaProvider
{
    public class KafkaProducer<T>
    {
        private string KafkaServerURL { get; set; }
        private string Topic { get; set; }
        private ProducerConfig Config { get; set; }
        public Action<DeliveryReport<Null, T>> ReportHandler { get; }
        public EventHandler<string> OnError;
        public EventHandler<string> ReportDelivery;
        private KafkaSerializer<T> Serializer { get; }

        public KafkaProducer(string kafkaServerURL, string topic, KafkaSerializer<T> serializer)
        {
            Serializer = serializer;
            KafkaServerURL = kafkaServerURL;
            Topic = topic;
            Config = new ProducerConfig
            {
                BootstrapServers = KafkaServerURL,
                MessageTimeoutMs = 5000
            };
            ReportHandler = r =>
            {
                if (r.Error.IsError)
                    OnError?.Invoke(this, $"Delivery Error: {r.Error.Reason}");
                else
                    ReportDelivery?.Invoke(this, $"Delivered to {r.TopicPartitionOffset}. Topic: {r.Topic}");
            };

        }

        public async Task<DeliveryResult<Null, T>> PublishAsync(T message)
        {
            using (var p = new ProducerBuilder<Null, T>(Config).SetValueSerializer(Serializer).Build())
            {
                DeliveryResult<Null, T> dr = await p.ProduceAsync(Topic, new Message<Null, T> { Value = message });
                return dr;
            }
        }

        public void Publish(T message)
        {
            using (var p = new ProducerBuilder<Null, T>(Config).Build())
            {
                p.Produce(Topic, new Message<Null, T> { Value = message }, ReportHandler);
            }
        }

    }
}
