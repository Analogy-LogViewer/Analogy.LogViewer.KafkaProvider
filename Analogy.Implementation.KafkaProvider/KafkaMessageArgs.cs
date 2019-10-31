using System;
using System.Collections.Generic;
using System.Text;
using Analogy.Interfaces;

namespace Analogy.Implementation.KafkaProvider
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
