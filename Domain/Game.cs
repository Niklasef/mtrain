using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Domain
{
    public static class Game
    {
        public static void Create(ushort playerCount)
        {
            MexicanTrain = null;
            var tiles = new ShuffledTileSetFactory().Create();

            var doubleTile = tiles.First(tile => tile.FirstValue == 12 && tile.SecondValue == 12);
            tiles.Remove(doubleTile);
            Engine = doubleTile;
            Engine.State = new EngineState();
            var players = new List<Player>();
            for (var i = 0; i < playerCount; i++)
            {
                players.Add(new Player(Engine));
            }

            Players = players;

            foreach (var player in Players)
            {
                for (int i = 0; i < 10; i++)
                {
                    var tile = tiles.First();
                    tiles.Remove(tile);
                    player.AddTile(tile);
                }
            }
            Boneyard = tiles;
        }
        public static IEnumerable<Player> Players { get; private set; }
        public static ITrain MexicanTrain { get; private set; }
        public static DominoTile Engine { get; private set; }
        internal static ICollection<DominoTile> Boneyard { get; private set; }
    }
}