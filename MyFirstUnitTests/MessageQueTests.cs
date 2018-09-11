using Xunit;
using Domain;
using System.Collections.Generic;
using System.Threading;

namespace MyFirstUnitTests
{
    public class MessageQueTests{
        private class FakeSubscriber : ISubscriber{
            public IList<Message> Messages;
            public void Process(Message message){
                Messages.Add(message);
            }
        }

        [Fact]        
        public void Subscribing_OneMessageAdded_MessageProcessed(){
            var subscriber = new FakeSubscriber();
            subscriber.Messages = new List<Message>();
            var que = new MessageQue();
            var m = new Message();
            
            que.Subscribe(subscriber);
            que.Add(m);
            Thread.Sleep(200);
            que.Dispose();

            Assert.Single(subscriber.Messages);
        }

        [Fact]        
        public void Subscribing_TwoMessageAdded_MessageProcessed(){
            var subscriber = new FakeSubscriber();
            subscriber.Messages = new List<Message>();
            var que = new MessageQue();
            
            que.Subscribe(subscriber);
            que.Add(new Message());
            que.Add(new Message());
            Thread.Sleep(200);
            que.Dispose();

            Assert.Equal(2, subscriber.Messages.Count());
        }

        [Fact]
        public void Subscribing_TwoMessagesTwoSubscribers_MessageProcessed(){
            var subscriberOne = new FakeSubscriber();
            var subscriberTwo = new FakeSubscriber();
            subscriberOne.Messages = new List<Message>();
            subscriberTwo.Messages = new List<Message>();
            var que = new MessageQue();
        
            que.Subscribe(subscriberOne);
            que.Subscribe(subscriberTwo);
            que.Add(new Message());
            que.Add(new Message());
            Thread.Sleep(200);
            que.Dispose();

            Assert.Equal(2, subscriberOne.Messages.Count());
            Assert.Equal(2, subscriberTwo.Messages.Count());            
        }

    }
}
