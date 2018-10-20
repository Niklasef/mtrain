using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Domain
{
    public static class Games
    {
        private static readonly IDictionary<Guid, MexicanTrainGame> innerList = new Dictionary<Guid, MexicanTrainGame>();

        public static MexicanTrainGame Get(Guid key)
        {
            if(!innerList.ContainsKey(key))
            {
                throw new ApplicationException($"Game with id {key} not added.");
            }
            return innerList[key];
        }

        public static void Add(Guid key, MexicanTrainGame value)
        {
            innerList.Add(key, value);
        }

        internal static void Remove(Guid id)
        {
            innerList.Remove(id);
        }
    }

    public class MexicanTrainGame
    {
        public Guid Id { get; private set; }
        public IEnumerable<Player> Players { get; private set; }
        public ITrain MexicanTrain { get; private set; }
        public DominoTile Engine { get; private set; }
        internal ICollection<DominoTile> Boneyard { get; private set; }

        protected internal MexicanTrainGame(
            Guid id,
            IEnumerable<Player> players,
            ITrain mexicanTrain,
            DominoTile engine,
            ICollection<DominoTile> boneyard)
        {
            Id = id;
            Players = players ?? throw new ArgumentNullException(nameof(players));
            MexicanTrain = mexicanTrain ?? throw new ArgumentNullException(nameof(mexicanTrain));
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            Boneyard = boneyard ?? throw new ArgumentNullException(nameof(boneyard));
        }

        public static MexicanTrainGame Create(HashSet<string> playerNames)
        {
            var gameId = Guid.NewGuid();
            var tiles = new ShuffledTileSetFactory().Create();
            var engineTile = tiles.First(tile => tile.FirstValue == 12 && tile.SecondValue == 12);
            tiles.Remove(engineTile);
            engineTile.State = new EngineState();
            var players = new List<Player>();
            foreach (var playerName in playerNames)
            {
                var playerTiles = tiles
                    .Take(10)
                    .ToArray();
                players.Add(new Player(gameId, playerName, new HashSet<DominoTile>(playerTiles)));
                foreach (var tile in playerTiles)
                {
                    tiles.Remove(tile);
                }
            }
            players.First().GiveTurn();

            var game = new MexicanTrainGame(
                gameId,
                players,
                new MexicanTrain(),
                engineTile,
                tiles);
            Games.Add(game.Id, game);
            return game;
        }

        internal void PassMove(Guid playerId)
        {
            var tile = Boneyard.First();
            Boneyard.Remove(tile);
            GetPlayer(playerId)
                .PassMove(tile);
        }

        private Player GetPlayer(Guid playerId)
        {
            return Players
                .First(p => p.Id == playerId);
        }

        internal void MakeMove(Guid playerId, long tileId, Guid trainId)
        {
            var train = GetTrain(trainId);
            GetPlayer(playerId)
                .MakeMove(tileId, train);
        }

        internal ITrain GetTrain(Guid trainId)
        {
            return MexicanTrain.Id == trainId
                ? MexicanTrain
                : Players
                    .First(p => p.Train.Id == trainId)
                    .Train;
        }

    }
}