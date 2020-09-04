using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.Implementation.KafkaProvider.Example
{
    public class AnalogyKafkaExampleDataProvider : IAnalogyRealTimeDataProvider
    {
        public string OptionalTitle => "Real time kafka provider example";
        public Guid Id { get; } = Guid.Parse("F1283D38-01B9-4753-996B-3CEF4312D7E2");
        public Image ConnectedLargeImage { get; } = null;
        public Image ConnectedSmallImage { get; } = null;
        public Image DisconnectedLargeImage { get; } = null;
        public Image DisconnectedSmallImage { get; } = null;
        public event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
        public event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
        public event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;
        public IAnalogyOfflineDataProvider FileOperationsHandler { get; }
        public bool IsConnected { get; private set; }
        public KafkaConsumer<AnalogyLogMessage> Consumer { get; set; }
        public KafkaProducer<AnalogyLogMessage> Producer { get; set; }



        public string groupId = "AnalogyKafkaExample";
        public string topic = "KafkaLog";
        public string kafkaUrl = "localhost:9092";
        public Task<bool> CanStartReceiving() => Task.FromResult(IsConnected);
        private TimerMessagesSimulator sim;
        private Task Consuming;
        public bool UseCustomColors { get; set; } = false;
        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public AnalogyKafkaExampleDataProvider()
        {

        }
        public Task StartReceiving()
        {
            sim.Start();
            Consuming = Consumer.StartConsuming();
            return Task.CompletedTask;
        }

        public Task StopReceiving()
        {
            sim.Stop();
            Consumer.StopConsuming();
            Consumer.OnMessageReady -= Consumer_OnMessageReady;
            Consumer.OnError -= Consumer_OnError;
            return Task.CompletedTask;
        }

        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
        {
            Producer = new KafkaProducer<AnalogyLogMessage>(kafkaUrl, topic, new KafkaSerializer<AnalogyLogMessage>());
            Consumer = new KafkaConsumer<AnalogyLogMessage>(groupId, kafkaUrl, topic);
            Consumer.OnMessageReady += Consumer_OnMessageReady;
            Consumer.OnError += Consumer_OnError;
            sim = new TimerMessagesSimulator(async m => { await Producer.PublishAsync(m); });
            IsConnected = true;
            return Task.CompletedTask;
        }

        public void MessageOpened(AnalogyLogMessage message)
        {
            //nop
        }

        private void Consumer_OnError(object sender, KafkaMessageArgs<string> e)
        {
            AnalogyLogMessage error = new AnalogyLogMessage(e.Message, AnalogyLogLevel.Error, AnalogyLogClass.General, Environment.MachineName);
            OnMessageReady?.Invoke(sender, new AnalogyLogMessageArgs(error, Environment.MachineName, Environment.MachineName, Id));
        }

        private void Consumer_OnMessageReady(object sender, KafkaMessageArgs<AnalogyLogMessage> e)
        {
            OnMessageReady?.Invoke(sender, new AnalogyLogMessageArgs(e.Message, Environment.MachineName, Environment.MachineName, Id));
        }

    }
}
