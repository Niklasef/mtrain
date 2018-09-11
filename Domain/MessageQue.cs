using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace Domain
{
    public class AsyncMessageQue : IDisposable{
        private readonly IList<ISubscriber> subscribers;
        private readonly IList<Message> messages;
        private readonly Timer timer;
        public AsyncMessageQue(){
            this.subscribers = new List<ISubscriber>();
            this.messages = new List<Message>();
            
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMilliseconds(100);

            this.timer = new Timer((e) => {
                Process();   
            }, null, startTimeSpan, periodTimeSpan);
        }
        public Func<object> Add(Message message){
            this.messages.Add(message);
            return () => {
                message.SyncEvent.WaitOne();
                return message.Result;
            };
        }

        public void Dispose() {
            this.timer.Dispose();
        }

        public void Subscribe(ISubscriber subscriber){
            this.subscribers.Add(subscriber);
        }

        private void Process(){
            if(this.messages.Count() == 0){
                return;
            }
            foreach(var subscriber in this.subscribers){
                var messageThread = new Thread(() => {
                    var message = this.messages.First();
                    subscriber.Process(message);
                    message.Result = 45;
                    this.messages.Remove(message);
                    message.SyncEvent.Set();
                });
                messageThread.Start();
            }
        }
    }

    public class MessageQue : IDisposable{
        private readonly IList<ISubscriber> subscribers;
        private readonly IList<Message> messages;
        private readonly Timer timer;
        public MessageQue(){
            this.subscribers = new List<ISubscriber>();
            this.messages = new List<Message>();
            
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMilliseconds(100);

            this.timer = new Timer((e) => {
                Process();   
            }, null, startTimeSpan, periodTimeSpan);
        }
        public void Add(Message message){
            this.messages.Add(message);
        }

        public void Dispose() {
            this.timer.Dispose();
        }

        public void Subscribe(ISubscriber subscriber){
            this.subscribers.Add(subscriber);
        }

        private void Process(){
            if(this.messages.Count() == 0){
                return;
            }
            while(this.messages.Any()){
                var message = this.messages.First();
                foreach(var subscriber in this.subscribers){
                    subscriber.Process(message);
                }
                this.messages.Remove(message);
            }
        }
    }
}