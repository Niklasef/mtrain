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
            if (!innerList.ContainsKey(key))
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
        private GameState state = new NoDoublesGameState();

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

        internal void MakeMove(Guid playerId, long tileId, Guid trainId)
        {
            state.MakeMove(this, playerId, tileId, trainId);
        }

        internal void PassMove(Guid playerId)
        {
            state.PassMove(this, playerId);
        }

        private abstract class GameState
        {
            internal abstract void MakeMove(MexicanTrainGame mexicanTrainGame, Guid playerId, long tileId, Guid trainId);
            internal abstract void PassMove(MexicanTrainGame mexicanTrainGame, Guid playerId);

            protected void PassTurn(MexicanTrainGame game, Guid currentPlayerId)
            {
                var nextPlayer = game
                    .Players
                    .SkipWhile(p => p.Id != currentPlayerId)
                    .Skip(1)
                    .FirstOrDefault() ?? game.Players.First();
                game
                    .Players
                    .First(p => p.Id == currentPlayerId)
                    .EndTurn();
                nextPlayer
                    .GiveTurn();
            }

            protected Player GetPlayer(MexicanTrainGame game, Guid playerId)
            {
                return game.Players
                    .First(p => p.Id == playerId);
            }

            protected ITrain GetTrain(MexicanTrainGame game, Guid trainId)
            {
                return game.MexicanTrain.Id == trainId
                    ? game.MexicanTrain
                    : game
                        .Players
                        .First(p => p.Train.Id == trainId)
                        .Train;
            }
        }

        private class OpenDoubleGameState : GameState
        {
            internal override void MakeMove(MexicanTrainGame mexicanTrainGame, Guid playerId, long tileId, Guid trainId)
            {
                throw new NotImplementedException();
            }

            internal override void PassMove(MexicanTrainGame mexicanTrainGame, Guid playerId)
            {
                throw new NotImplementedException();
            }
        }
        private class NoDoublesGameState : GameState
        {
            internal override void MakeMove(MexicanTrainGame game, Guid playerId, long tileId, Guid trainId)
            {
                var train = GetTrain(game, trainId);
                GetPlayer(game, playerId)
                    .MakeMove(tileId, train);
                if (GetPlayedTile(game, tileId).IsDouble())
                {
                    game.state = new OpenDoubleGameState();
                    return;
                }
                PassTurn(game, playerId);
            }

            private DominoTile GetPlayedTile(MexicanTrainGame game, long tileId)
            {
                return game
                    .Players
                    .SelectMany(p => p
                        .Train
                        .GetTiles())
                    .Union(game
                        .MexicanTrain
                        .GetTiles())
                    .First(t => t.Id == tileId);
            }

            override internal void PassMove(MexicanTrainGame game, Guid playerId)
            {
                var tile = game
                    .Boneyard
                    .First();
                game.Boneyard
                    .Remove(tile);
                GetPlayer(game, playerId)
                    .PassMove(tile);
                PassTurn(game, playerId);
            }
        }
    }

}