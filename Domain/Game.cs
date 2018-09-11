using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Domain
{

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