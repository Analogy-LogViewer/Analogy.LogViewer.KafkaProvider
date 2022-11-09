using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.LogViewer.KafkaProvider
{
    public class AnalogyKafkaDataProvider : Template.OnlineDataProvider
    {
        public override Guid Id { get; set; } = Guid.Parse("350A2268-DAB2-4991-A29F-F597DD6E52FA");
        public override string OptionalTitle { get; set; } = "Real time Kafka provider";


        public override IAnalogyOfflineDataProvider FileOperationsHandler { get; set; }
        public bool IsConnected { get; private set; }
        public KafkaConsumer<AnalogyLogMessage> Consumer { get; set; }
        public string groupId = "AnalogyKafkaLogin";
        public string topic = "KafkaLog";
        public string kafkaUrl = "localhost:9092";
        public override Task<bool> CanStartReceiving() => Task.FromResult(IsConnected);
        private Task Consuming;
        public override bool UseCustomColors { get; set; } = false;
        public override IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public override (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public AnalogyKafkaDataProvider()
        {

        }
        public override Task StartReceiving()
        {
            Consuming = Consumer.StartConsuming();
            return Task.CompletedTask;
        }

        public override Task StopReceiving()
        {
            Consumer.StopConsuming();
            return Task.CompletedTask;
        }


        public override Task InitializeDataProvider(IAnalogyLogger logger)
        {
            Consumer = new KafkaConsumer<AnalogyLogMessage>(groupId, kafkaUrl, topic);
            Consumer.OnMessageReady += Consumer_OnMessageReady;
            IsConnected = true;
            return base.InitializeDataProvider(logger);
        }

        public override Task ShutDown() => Task.CompletedTask;
        public override void MessageOpened(AnalogyLogMessage message)
        {
            //nop
        }
        private void Consumer_OnMessageReady(object sender, KafkaMessageArgs<AnalogyLogMessage> e)
        {
            MessageReady(sender, new AnalogyLogMessageArgs(e.Message, Environment.MachineName, Environment.MachineName, Id));
        }

    }
}
