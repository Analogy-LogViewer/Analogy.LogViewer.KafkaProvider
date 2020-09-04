﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.LogViewer.KafkaProvider
{
    public class AnalogyKafkaDataProvider : IAnalogyRealTimeDataProvider
    {
        public Guid Id { get; set; } = Guid.Parse("350A2268-DAB2-4991-A29F-F597DD6E52FA");
        public string OptionalTitle { get; set; } = "Real time Kafka provider";

        public event EventHandler<AnalogyDataSourceDisconnectedArgs> OnDisconnected;
        public event EventHandler<AnalogyLogMessageArgs> OnMessageReady;
        public event EventHandler<AnalogyLogMessagesArgs> OnManyMessagesReady;
        public IAnalogyOfflineDataProvider FileOperationsHandler { get; }
        public Image ConnectedLargeImage { get; set; } = null;
        public Image ConnectedSmallImage { get; set; } = null;
        public Image DisconnectedLargeImage { get; set; } = null;
        public Image DisconnectedSmallImage { get; set; } = null;
        public bool IsConnected { get; private set; }
        public KafkaConsumer<AnalogyLogMessage> Consumer { get; set; }
        public string groupId = "AnalogyKafkaLogin";
        public string topic = "KafkaLog";
        public string kafkaUrl = "localhost:9092";
        public Task<bool> CanStartReceiving() => Task.FromResult(IsConnected);
        private Task Consuming;
        public bool UseCustomColors { get; set; } = false;
        public IEnumerable<(string originalHeader, string replacementHeader)> GetReplacementHeaders()
            => Array.Empty<(string, string)>();

        public (Color backgroundColor, Color foregroundColor) GetColorForMessage(IAnalogyLogMessage logMessage)
            => (Color.Empty, Color.Empty);
        public AnalogyKafkaDataProvider()
        {

        }
        public Task StartReceiving()
        {
            Consuming = Consumer.StartConsuming();
            return Task.CompletedTask;
        }

        public Task StopReceiving()
        {
            Consumer.StopConsuming();
            return Task.CompletedTask;
        }


        public Task InitializeDataProviderAsync(IAnalogyLogger logger)
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
            OnMessageReady?.Invoke(sender, new AnalogyLogMessageArgs(e.Message, Environment.MachineName, Environment.MachineName, Id));
        }

    }
}
