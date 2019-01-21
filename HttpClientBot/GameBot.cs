using System;
using System.Collections.Generic;
using HttpClient;
using Domain.Game;
using System.Linq;
using System.Threading;
using System.Net.Http;

namespace HttpClientBot
{
    public class GameBot
    {
        private readonly Guid gameId;
        private readonly Guid playerId;
        private readonly IMoveStrategy moveStrategy;
        private readonly GameHttpClient gameClient;
        private readonly object padLock = new object();
        private GameBoard gameBoard;

        public GameBot(
            Guid gameId,
            Guid playerId,
            GameHttpClient gameClient,
            IMoveStrategy moveStrategy
        )
        {
            this.gameId = gameId;
            this.playerId = playerId;
            this.gameClient = gameClient;
            this.moveStrategy = moveStrategy;
        }

        public static GameBot Create(Guid gameId)
        {
            var gameClient = new GameHttpClient(new System.Net.Http.HttpClient());
            var gameBoards = gameClient.GetGameBoards();
            if (!gameBoards.Any(gb => gb.GameId == gameId))
            {
                gameClient.CreateGame(gameId);
            }
            var playerId = Guid.NewGuid();
            gameClient.JoinGame(
                gameId,
                playerId,
                $"bot-{playerId}");
            return new GameBot(
                gameId,
                playerId,
                gameClient,
                RandomMoveStrategy.Create());
        }

        public void Start()
        {
            var periodTimeSpan = TimeSpan.FromSeconds(1);

            new Timer((e) =>
            {
                RefreshGameBoard();
            }, null, TimeSpan.Zero, periodTimeSpan);

            new Timer((e) =>
            {
                DoMove();
            }, null, TimeSpan.Zero, periodTimeSpan);
        }

        private void DoMove()
        {
            lock (padLock)
            {
                if (gameBoard?.PlayerIdWithTurn == playerId)
                {
                    moveStrategy
                        .Execute(
                            playerId,
                            gameId,
                            gameBoard);
                }
            }
        }

        private void RefreshGameBoard()
        {
            lock (padLock)
            {
                gameBoard = gameClient
                    .GetGameBoard(gameId, playerId);
            }
        }
    }

    internal class RandomMoveStrategy : IMoveStrategy
    {
        private Random random = new Random();
        private GameHttpClient gameHttpClient;

        public RandomMoveStrategy(GameHttpClient gameHttpClient)
        {
            this.gameHttpClient = gameHttpClient;
        }

        internal static IMoveStrategy Create()
        {
            return new RandomMoveStrategy(new GameHttpClient(new System.Net.Http.HttpClient()));
        }

        public void Execute(
            Guid playerId,
            Guid gameId,
            GameBoard gameBoard)
        {
            for (var i = 0; i < 10; i++)
            {
                try
                {
                    var trainIds = gameBoard
                        .PlayerTrains
                        .Keys
                        .Union(new[]{gameBoard
                            .MexicanTrain
                            .Key});
                    var trainCount = trainIds.Count();
                    var trainIdToPlay = trainIds
                        .Skip(random.Next(0, trainCount--))
                        .First();

                    var tileCount = gameBoard.Hand.Count();
                    var tileIdToPlay = gameBoard
                        .Hand
                        .Skip(random.Next(0, tileCount--))
                        .First()
                        .Id;

                    gameHttpClient.MakeMove(
                        gameId,
                        playerId,
                        trainIdToPlay,
                        tileIdToPlay);
                }
                catch
                {

                }
            }
            gameHttpClient.PassMove(
                gameId,
                playerId);
        }
    }

    public interface IMoveStrategy
    {
        void Execute(
            Guid playerId,
            Guid gameId,
            GameBoard gameBoard);
    }
}