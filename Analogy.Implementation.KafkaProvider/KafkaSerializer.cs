#pragma warning disable SYSLIB0011

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Confluent.Kafka;

namespace Analogy.LogViewer.KafkaProvider
{
    public class KafkaSerializer<T> : ISerializer<T>, IDeserializer<T>
    {
        private readonly BinaryFormatter _bFormatter;
        public KafkaSerializer()
        {
            _bFormatter = new BinaryFormatter();
        }

        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            using (var m = new MemoryStream())
            {
                m.Write(data.ToArray(), 0, data.Length);
                m.Position = 0;
                return (T)_bFormatter.Deserialize(m);
            }
        }
        public byte[] Serialize(T data, SerializationContext context)
        {
            using (var m = new MemoryStream())
            {
                _bFormatter.Serialize(m, data);
                m.Position = 0;
                return m.ToArray();
            }

        }

    }
}
