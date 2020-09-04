using Analogy.Interfaces;

namespace Analogy.LogViewer.KafkaProvider
{
    class AnalogyKafkaProducer : KafkaProducer<AnalogyLogMessage>
    {
        public AnalogyKafkaProducer(string kafkaServerURL, string topic) : base(kafkaServerURL, topic, new KafkaSerializer<AnalogyLogMessage>())
        {
        }
    }
}
