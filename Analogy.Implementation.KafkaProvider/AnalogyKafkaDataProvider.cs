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
        public string OptionalTitle { get; } = "Real time Kafka provider";
        public event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
        public event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
        public event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;
        public IAnalogyOfflineDataProvider FileOperationsHandler { get; }
        public bool IsConnected { get; private set; }
        public KafkaConsumer<AnalogyLogMessage> Consumer { get; set; }
        public string groupId = "AnalogyKafkaLogin";
        public string topic = "KafkaLog";
        public string kafkaUrl = "localhost:9092";
        public Task<bool> CanStartReceiving() => Task.FromResult(IsConnected);
        private Task Consuming;
        public AnalogyKafkaDataProvider()
        {

        }
        public void StartReceiving()
        {
            Consuming = Consumer.StartConsuming();
        }

        public void StopReceiving()
        {
            Consumer.StopConsuming();
        }

       
        public Task InitializeDataProviderAsync()
        {
            Consumer = new KafkaConsumer<AnalogyLogMessage>(groupId, kafkaUrl, topic);
            Consumer.OnMessageReady += Consumer_OnMessageReady;
            IsConnected = true;
            return Task.CompletedTask;
        }

        public void MessageOpened(AnalogyLogMessage message)
        {
          //nop
        }
        private void Consumer_OnMessageReady(object sender, KafkaMessageArgs<AnalogyLogMessage> e)
        {
            OnMessageReady?.Invoke(sender, new AnalogyLogMessageArgs(e.Message, Environment.MachineName, Environment.MachineName, ID));
        }

    }
}
