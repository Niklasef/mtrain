using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Domain
{
    public static class MexicanTrainGame
    {
        public static void Create(HashSet<string> playerNames)
        {
            MexicanTrain = null;
            var tiles = new ShuffledTileSetFactory().Create();

            var doubleTile = tiles.First(tile => tile.FirstValue == 12 && tile.SecondValue == 12);
            tiles.Remove(doubleTile);
            Engine = doubleTile;
            Engine.State = new EngineState();
            var players = new List<Player>();
            foreach (var playerName in playerNames)
            {
                var playerTiles = tiles
                    .Take(10)
                    .ToArray();
                players.Add(new Player(Engine, playerName, new HashSet<DominoTile>(playerTiles)));
                foreach(var tile in playerTiles)
                {
                    tiles.Remove(tile);
                }
            }

            Players = players;
            Boneyard = tiles;
        }
        public static IEnumerable<Player> Players { get; private set; }
        public static ITrain MexicanTrain { get; private set; }
        public static DominoTile Engine { get; private set; }
        internal static ICollection<DominoTile> Boneyard { get; private set; }
    }
}