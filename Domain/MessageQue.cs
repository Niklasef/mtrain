using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace Domain
{
    public class MessageQue : IDisposable
    {
        private readonly IList<ISubscriber> subscribers;
        private readonly IList<Message> messages;
        private readonly Timer timer;
        public MessageQue()
        {
            this.subscribers = new List<ISubscriber>();
            this.messages = new List<Message>();

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMilliseconds(100);

            this.timer = new Timer((e) =>
            {
                Process();
            }, null, startTimeSpan, periodTimeSpan);
        }
        public void Add(Message message)
        {
            this.messages.Add(message);
        }

        public void Dispose()
        {
            this.timer.Dispose();
        }

        public void Subscribe(ISubscriber subscriber)
        {
            this.subscribers.Add(subscriber);
        }

        private void Process()
        {
            if (this.messages.Count() == 0)
            {
                return;
            }
            while (this.messages.Any())
            {
                var message = this.messages.First();
                foreach (var subscriber in this.subscribers)
                {
                    subscriber.Process(message);
                }
                this.messages.Remove(message);
            }
        }
    }
}