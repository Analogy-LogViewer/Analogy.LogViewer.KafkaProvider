using System;
using System.Collections.Generic;
using System.Text;
using Analogy.Interfaces;

namespace Analogy.Implementation.KafkaProvider
{
    class AnalogyKafkaProducer : KafkaProducer<AnalogyLogMessage>
    {
        public AnalogyKafkaProducer(string kafkaServerURL, string topic) : base(kafkaServerURL, topic, new KafkaSerializer<AnalogyLogMessage>())
        {
        }
    }
}
