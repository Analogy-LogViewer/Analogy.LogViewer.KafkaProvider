using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;
using Confluent.Kafka;

namespace Analogy.Implementation.KafkaProvider
{
    public class KafkaConsumer<T>
    {
        private string KafkaServerURL { get; set; }
        private string Topic { get; set; }
        private ConsumerConfig Config { get; set; }
        public event EventHandler<KafkaMessageArgs<T>> OnMessageReady;
        public BlockingCollectionQueue<T> Queue;
        public BlockingCollectionQueue<string> ErrorsQueue;
        private readonly KafkaSerializer<T> serializer;
        private readonly CancellationTokenSource cts;
        public KafkaConsumer(string groupId, string kafkaServerURL, string topic)
        {
            serializer = new KafkaSerializer<T>();
            cts = new CancellationTokenSource();
            Queue = new BlockingCollectionQueue<T>();
            ErrorsQueue = new BlockingCollectionQueue<string>();
            KafkaServerURL = kafkaServerURL;
            Topic = topic;
            Config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = KafkaServerURL,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };


        }

        private Task ConsumeAsync()
        {
            return Task.Factory.StartNew(() =>
             {
                 using (var c = new ConsumerBuilder<Ignore, T>(Config).SetValueDeserializer(serializer).Build())
                 {
                     c.Subscribe(Topic);
                     try
                     {
                         while (true)
                         {
                             try
                             {
                                 var cr = c.Consume(cts.Token);
                                 Queue.Enqueue(cr.Value);
                             }
                             catch (TaskCanceledException ce)
                             {
                                 Queue.CompleteAdding();
                                 return;
                             }
                             catch (ConsumeException e)
                             {
                                 ErrorsQueue.Enqueue($"Error occurred: {e.Error.Reason}");
                             }
                         }
                     }
                     catch (OperationCanceledException)
                     {
                         // Ensure the consumer leaves the group cleanly and final offsets are committed.
                         c.Close();
                     }
                 }
             });

        }

        private Task ReadAsync() => Task.Factory.StartNew(() =>
        {
            foreach (var item in Queue.GetConsumingEnumerable(cts.Token))
            {
                OnMessageReady?.Invoke(this, new KafkaMessageArgs<T>(item));
            }
        });

        public void StopConsuming()
        {
            cts.Cancel();
        }


        public Task StartConsuming() => Task.WhenAll(ConsumeAsync(), ReadAsync());

    }
}
