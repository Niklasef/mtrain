﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Domain.DominoTile;
using Domain.Train;
using Domain.Player;

namespace Domain.Game
{
    public partial class MexicanTrainGame
    {
        private class OpeningDoubleGameState : GameStateBase
        {
            private readonly List<Tuple<Guid, Guid>> openTrains;

            public OpeningDoubleGameState(Tuple<Guid, Guid> trainAndPlayerId)
            {
                openTrains = new[] { trainAndPlayerId }.ToList();
            }

            internal override void MakeMove(
                MexicanTrainGame game,
                Guid playerId,
                long tileId,
                Guid trainId)
            {
                var train = game.GetTrain(trainId);
                game.GetPlayer(playerId)
                    .ForceMove(tileId, train);//TODO: Possible to move this to shared security assembly?
                if (game.GetPlayedTile(tileId).IsDouble())
                {
                    openTrains.Add(new Tuple<Guid, Guid>(trainId, playerId));
                    return;
                }
                game.state = openTrains.Any()
                    ? game.state = new OpenedDoubleGameState(openTrains)
                    : new NoDoublesGameState();

                PassTurn(game, playerId);
            }

            internal override void PassMove(MexicanTrainGame game, Guid playerId)
            {
                var tile = game
                    .Boneyard
                    .First();
                game.Boneyard
                    .Remove(tile);
                game.GetPlayer(playerId)
                    .PassMove(tile);
                PassTurn(game, playerId);
            }
        }
    }

    public partial class MexicanTrainGame
    {
        public Guid Id { get; private set; }
        public IEnumerable<PlayerEntity> Players { get; private set; }
        public ITrain MexicanTrain { get; private set; }
        public DominoTileEntity Engine { get; private set; }
        internal ICollection<DominoTileEntity> Boneyard { get; private set; }
        private GameStateBase state = new NoDoublesGameState();
        public Type GetStateType()
        {
            return state.GetType();
        }

        protected internal MexicanTrainGame(
            Guid id,
            IEnumerable<PlayerEntity> players,
            ITrain mexicanTrain,
            DominoTileEntity engine,
            ICollection<DominoTileEntity> boneyard)
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
            var tiles = new ShuffledTileSetFactory().Create(12);
            var engineTile = tiles.First(tile => tile.FirstValue == 12 && tile.SecondValue == 12);
            tiles.Remove(engineTile);
            var players = new List<PlayerEntity>();
            foreach (var playerName in playerNames)
            {
                var playerTiles = tiles
                    .Take(10)
                    .ToArray();
                players.Add(new PlayerEntity(engineTile, playerName, new HashSet<DominoTileEntity>(playerTiles)));
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

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Game State: {state}");

            Players
                .ToList()
                .ForEach(p =>
                    stringBuilder.AppendLine(p.ToString()));

            stringBuilder.AppendLine("Mexican Train:");
            stringBuilder.AppendLine(
                MexicanTrain.ToString()
            );

            return stringBuilder.ToString();
        }

        internal PlayerEntity GetPlayer(Guid playerId) =>
            Players
                .First(p => p.Id == playerId);

        internal ITrain GetTrain(Guid trainId) =>
            MexicanTrain.Id == trainId
                ? MexicanTrain
                : Players
                    .First(p => p.Train.Id == trainId)
                    .Train;

        internal DominoTileEntity GetPlayedTile(long tileId) =>
            Players
                .SelectMany(p => p
                    .Train
                    .GetTiles())
                .Union(MexicanTrain
                    .GetTiles())
                .First(t => t.Id == tileId);
    }
}