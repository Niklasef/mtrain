using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Domain
{

    public interface ISubscriber {
        void Process(Message message);
    }

    public class Message{
        public Message(){
            SyncEvent = new ManualResetEvent(false);
            Id = Guid.NewGuid();
        }
        public object Result { get; set; }
        public ManualResetEvent SyncEvent { get; }
        public Guid Id{get;}

        public override bool Equals(object obj){
            if (obj == null || GetType() != obj.GetType()){
                return false;
            }
            
            return Id.Equals(((Message)obj).Id);
        }
        

        public override int GetHashCode(){
            return Id.GetHashCode();
        }
    }

    public class Game
    {
        public Game(IEnumerable<Player> players)
        {
            Players = players;
            MexicanTrain = null;
            var tiles = new ShuffledTileSetFactory().Create();

            var doubleTile = tiles.First(tile=>tile.FirstValue==12 && tile.SecondValue==12);
            tiles.Remove(doubleTile);
            Engine = doubleTile;

            foreach(var player in Players){
                for (int i = 0; i < 10; i++)
                {
                    var tile = tiles.First();
                    tiles.Remove(tile);
                    player.AddTile(tile);                    
                }
            }
            Boneyard = tiles;
        }
        public IEnumerable<Player> Players{get;}
        public ITrain MexicanTrain{get;}
        public DominoTile Engine{get;}
        internal ICollection<DominoTile> Boneyard{get;}
    }
}