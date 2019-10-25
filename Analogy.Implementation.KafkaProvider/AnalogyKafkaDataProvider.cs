using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.Implementation.KafkaProvider
{
    public class AnalogyKafkaDataProvider : IAnalogyRealTimeDataProvider
    {
        public Guid ID { get; } = Guid.Parse("350A2268-DAB2-4991-A29F-F597DD6E52FA");
        public event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
        public event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
        public event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;
        public IAnalogyOfflineDataProvider FileOperationsHandler { get; }
        public bool IsConnected { get; private set; }
        public KafkaConsumer Consumer { get; set; }
        public string topic = "KafkaLog";
        public string kafkaUrl = "localhost:9092";
        public Task<bool> CanStartReceiving() => Task.FromResult(IsConnected);
        private Task Consuming;
        private Task Reading;
        public AnalogyKafkaDataProvider()
        {

        }
        public void StartReceiving()
        {
            Consuming = Consumer.StartConsuming();
            Reading = Consumer.ReadMessages();

        }

        public void StopReceiving()
        {
            Consumer.StopConsuming();
        }

        public void InitDataProvider()
        {

            Consumer = new KafkaConsumer(kafkaUrl, topic);
            Consumer.OnMessageReady += Consumer_OnMessageReady;
            IsConnected = true;

        }

        private void Consumer_OnMessageReady(object sender, AnalogyKafkaLogMessageArgs e)
        {
            OnMessageReady?.Invoke(sender, new AnalogyLogMessageArgs(e.Message, Environment.MachineName, Environment.MachineName, ID));
        }

    }
}
