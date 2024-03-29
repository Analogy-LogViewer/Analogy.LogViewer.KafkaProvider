﻿using System;
using System.Timers;
using Analogy.Interfaces;

namespace Analogy.LogViewer.KafkaProvider.Example
{
    public class TimerMessagesSimulator
    {
        private int messageCount;
        private readonly Timer SimulateOnlineMessages;
        readonly Random random = new Random();
        readonly Array values = Enum.GetValues(typeof(AnalogyLogLevel));
        private Action<AnalogyLogMessage> ActionPerTick { get; }
        public TimerMessagesSimulator(Action<AnalogyLogMessage> action)
        {
            ActionPerTick = action;
            SimulateOnlineMessages = new Timer(100);
            SimulateOnlineMessages.Elapsed += (s, e) =>
            {
                unchecked
                {
                    AnalogyLogLevel randomLevel = (AnalogyLogLevel)values.GetValue(random.Next(values.Length));
                    AnalogyLogMessage m = new AnalogyLogMessage($"Generated message #{messageCount++}", randomLevel, AnalogyLogClass.General, "Example");
                    ActionPerTick(m);
                }
            };
        }

        public void Start() => SimulateOnlineMessages.Start();
        public void Stop() => SimulateOnlineMessages.Stop();
    }
}
