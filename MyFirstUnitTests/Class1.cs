using System;
using Xunit;
using Domain;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace MyFirstUnitTests
{
    public class FakeSubscriber : ISubscriber{
        public IList<Message> Messages;
        public void Process(Message message){
            Messages.Add(message);
        }
    }

    public class MessageQueTests{
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

    public class AsyncMessageQueTests{
        [Fact]        
        public void Subscribing_OneMessageAdded_MessageProcessed(){
            var subscriber = new FakeSubscriber();
            subscriber.Messages = new List<Message>();
            var que = new AsyncMessageQue();
            var m = new Message();
            
            que.Subscribe(subscriber);
            var result = que.Add(m).Invoke();
            Thread.Sleep(200);
            que.Dispose();

            Assert.Equal(45, result);
            Assert.Single(subscriber.Messages);
        }

    }
    public class ShuffledTileSetFactoryTests
    {
        [Fact]
        public void ShufleTiles_91Tiles()
        {
            Assert.Equal(
                91,
                new ShuffledTileSetFactory()
                    .Create()
                    .Count());
        }

        [Fact]
        public void ShufleTiles_TilesAreShuffled()
        {
            var tileStackOne = new ShuffledTileSetFactory().Create();
            var tileStackTwo = new ShuffledTileSetFactory().Create();
            Assert.False(Enumerable.SequenceEqual(tileStackOne, tileStackTwo));
        }

        [Fact]
        public void ShufleTiles_ContainsNoDuplicates()
        {
            var tileStack = new ShuffledTileSetFactory().Create();
            var duplicates = tileStack.Where(x => tileStack.Count(y => x.Equals(y)) > 1);
            Assert.True(
                duplicates.Count() == 0
            );
        }
    }

    public class GameTests
    {
        [Fact]
        public void CreatingGame_TwoPlayers_70TilesInBoneyard()
        {
            var boneyard = new Game(new []{new Player(),new Player()})
                .Boneyard;
            Assert.Equal(
                70,
                boneyard.Count());
        }

                [Fact]
        public void CreatingGame_TwoPlayers_10TilesEach()
        {
            var game = new Game(new []{new Player(),new Player()});
            Assert.Equal(
                10,
                game.Players.First().dominoTiles.Count());
            Assert.Equal(
                10,
                game.Players.Last().dominoTiles.Count());
        }
    }

    public class DominoTests
    {
        [Fact]
        public void Flipping_SumRemainsSame(){
            var tile = new DominoTile(1, 10);
            var tileTwo = new DominoTile(1, 10);
            tileTwo.Flip();

            Assert.Equal(tile, tileTwo);
        }
    }
}
