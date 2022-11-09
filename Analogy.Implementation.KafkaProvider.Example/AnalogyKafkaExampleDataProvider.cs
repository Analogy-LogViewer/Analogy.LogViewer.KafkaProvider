using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.LogViewer.KafkaProvider.Example
{
    public class AnalogyKafkaExampleDataProvider : Template.OnlineDataProvider
    {
        public override string OptionalTitle { get; set; } = "Real time kafka provider example";
        public override Guid Id { get; set; } = Guid.Parse("F1283D38-01B9-4753-996B-3CEF4312D7E2");
        public  override Image ConnectedLargeImage { get; set; } = null;
        public  override Image ConnectedSmallImage { get; set; } = null;
        public  override Image DisconnectedLargeImage { get; set; } = null;
        public override Image DisconnectedSmallImage { get; set; } = null;


        public override IAnalogyOfflineDataProvider FileOperationsHandler { get; set; }
        public bool IsConnected { get; private set; }
        public KafkaConsumer<AnalogyLogMessage> Consumer { get; set; }
        public KafkaProducer<AnalogyLogMessage> Producer { get; set; }
        
        public string groupId = "AnalogyKafkaExample";
        public string topic = "KafkaLog";
        public string kafkaUrl = "localhost:9092";
        public override Task<bool> CanStartReceiving() => Task.FromResult(IsConnected);
        private TimerMessagesSimulator sim;
        private Task Consuming;
        public override bool UseCustomColors { get; set; } = false;
        public override IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public override (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public AnalogyKafkaExampleDataProvider()
        {

        }
        public override Task StartReceiving()
        {
            sim.Start();
            Consuming = Consumer.StartConsuming();
            return Task.CompletedTask;
        }

        public override Task StopReceiving()
        {
            sim.Stop();
            Consumer.StopConsuming();
            Consumer.OnMessageReady -= Consumer_OnMessageReady;
            Consumer.OnError -= Consumer_OnError;
            return Task.CompletedTask;
        }
        public override Task ShutDown() => Task.CompletedTask;
        
        public override Task InitializeDataProvider(IAnalogyLogger logger)
        {
            Producer = new KafkaProducer<AnalogyLogMessage>(kafkaUrl, topic, new KafkaSerializer<AnalogyLogMessage>());
            Consumer = new KafkaConsumer<AnalogyLogMessage>(groupId, kafkaUrl, topic);
            Consumer.OnMessageReady += Consumer_OnMessageReady;
            Consumer.OnError += Consumer_OnError;
            sim = new TimerMessagesSimulator(async m => { await Producer.PublishAsync(m); });
            IsConnected = true;
            return base.InitializeDataProvider(logger);
        }

      
        private void Consumer_OnError(object sender, KafkaMessageArgs<string> e)
        {
            AnalogyLogMessage error = new AnalogyLogMessage(e.Message, AnalogyLogLevel.Error, AnalogyLogClass.General, Environment.MachineName);
            MessageReady(sender, new AnalogyLogMessageArgs(error, Environment.MachineName, Environment.MachineName, Id));
        }

        private void Consumer_OnMessageReady(object sender, KafkaMessageArgs<AnalogyLogMessage> e)
        {
            MessageReady(sender, new AnalogyLogMessageArgs(e.Message, Environment.MachineName, Environment.MachineName, Id));
        }

    }
}
