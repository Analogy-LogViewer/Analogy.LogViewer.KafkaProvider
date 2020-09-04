using System;

namespace Analogy.LogViewer.KafkaProvider
{
    public class KafkaMessageArgs<T> : EventArgs
    {
        public T Message { get; private set; }

        public KafkaMessageArgs(T msg)
        {
            Message = msg;
        }
    }
}
