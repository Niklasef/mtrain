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
    public partial class GameEntity
    {
        public Guid Id { get; private set; }
        public IEnumerable<PlayerEntity> Players => players;
        public ITrain MexicanTrain { get; private set; }
        public DominoTileEntity Engine { get; private set; }
        internal protected Stack<DominoTileEntity> boneyard;
        private List<PlayerEntity> players;
        private GameStateBase state = new NoDoublesGameState();
        private readonly List<Tuple<Guid, Guid, long>> openDoubles = new List<Tuple<Guid, Guid, long>>();//trainId, playerId, tileId


        public IEnumerable<long> GetOpenDoubleTileIds() =>
            openDoubles
                .Select(od => od.Item3)
                .ToArray();

        public Type GetStateType() =>
            state.GetType();

        protected internal GameEntity(
            Guid id,
            IEnumerable<PlayerEntity> players,
            ITrain mexicanTrain,
            DominoTileEntity engine,
            Stack<DominoTileEntity> boneyard)
        {
            Id = id;
            this.players = players?.ToList() ?? throw new ArgumentNullException(nameof(players));
            MexicanTrain = mexicanTrain ?? throw new ArgumentNullException(nameof(mexicanTrain));
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
            this.boneyard = boneyard ?? throw new ArgumentNullException(nameof(boneyard));
        }

        public static GameEntity Create(Guid gameId)
        {
            var tiles = new ShuffledTileSetFactory().Create(12);
            var engineTile = tiles
                .First(tile =>
                    tile.FirstValue == 12 &&
                    tile.SecondValue == 12);
            tiles.Remove(engineTile);
            // var players = new List<PlayerEntity>();
            // foreach (var playerName in playerNames)
            // {
            //     var playerTiles = tiles
            //         .Take(10)
            //         .ToArray();
            //     players.Add(new PlayerEntity(engineTile, gameId, playerName, new HashSet<DominoTileEntity>(playerTiles)));
            //     foreach (var tile in playerTiles)
            //     {
            //         tiles.Remove(tile);
            //     }
            // }
            // players
            //     .First()
            //     .GiveTurn();

            var game = new GameEntity(
                gameId,
                Enumerable.Empty<PlayerEntity>(),
                new MexicanTrain(),
                engineTile,
                new Stack<DominoTileEntity>(tiles));
            Games.Add(game.Id, game);
            return game;
        }

        public void MakeMove(Guid playerId, long tileId, Guid trainId) =>
            state.MakeMove(this, playerId, tileId, trainId);

        public void PassMove(Guid playerId) =>
            state.PassMove(this, playerId);

        public void AddPlayer(Guid playerId, string playerName) =>
            state.AddPlayer(this, playerId, playerName);

        public void Start() =>
            state.Start(this);

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Game State: '{state}'");
            stringBuilder.AppendLine($"Open tiles: '{string.Join(", ", GetOpenDoubleTileIds().Select(tId => GetPlayedTile(tId)))}'");

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

        public GameBoard GetBoard(Guid playerId) =>
            new GameBoard(
                Players
                    .ToDictionary(p => p.Id, p => p.Name),
                GetStateType(),
                Id,
                GetTrains(),
                GetPlayer(playerId)
                    .Hand,
                Players
                    .First(p => p
                        .GetStateType()
                        .Name
                        .Equals("HasTurnPlayerState", StringComparison.Ordinal))
                    .Id);

        public GameBoard GetBoard() =>
            new GameBoard(
                Players
                    .ToDictionary(p => p.Id, p => p.Name),
                GetStateType(),
                Id,
                GetTrains(),
                Enumerable.Empty<DominoTileEntity>(),
                Players
                    .First(p => p
                        .GetStateType()
                        .Name
                        .Equals("HasTurnPlayerState", StringComparison.Ordinal))
                    .Id);

        internal PlayerEntity GetPlayer(Guid playerId) =>
            Players
                .First(p => p.Id == playerId);

        internal ITrain GetTrain(Guid trainId) =>
            MexicanTrain.Id == trainId
                ? MexicanTrain
                : Players
                    .First(p => p.Train.Id == trainId)
                    .Train;

        internal IEnumerable<ITrain> GetTrains() =>
            new[] { MexicanTrain }
                .Union(
                    Players
                        .Select(p => p
                            .Train));

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