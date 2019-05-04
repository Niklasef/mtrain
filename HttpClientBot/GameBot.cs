using System;
using System.Collections.Generic;
using HttpClient;
using Domain.Game;
using System.Linq;
using System.Threading;
using System.Net.Http;

namespace HttpClientBot
{
    public class GameBot : IDisposable
    {
        private readonly Guid gameId;
        private readonly Guid playerId;
        private readonly IMoveStrategy moveStrategy;
        private Timer timer;
        private readonly GameHttpClient gameClient;
        private GameBoard gameBoard;

        public event OnExceptionHandler OnException;
        public delegate void OnExceptionHandler(Exception exception);
        private void FireOnException(Exception exception) => OnException?.Invoke(exception);

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
                //gameClient.CreateGame(gameId);
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
            timer = new Timer((e) =>
            {
                try
                {
                    RefreshGameBoard();
                }
                catch (Exception exception)
                {
                    Console.Write(exception);
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void TryDoMove()
        {
            if (gameBoard?.PlayerIdWithTurn == playerId)
            {
                doingMove = true;
                Console.WriteLine("Trying to do move");
                try
                {
                    moveStrategy
                        .Execute(
                            playerId,
                            gameId,
                            gameBoard);
                }
                finally
                {
                    doingMove = false;
                }
            }
        }

        private bool doingMove = false;

        private void RefreshGameBoard()
        {
            if (doingMove)
            {
                return;
            }
            Console.WriteLine($"Refreshing board");
            gameBoard = gameClient
                .GetGameBoard(gameId, playerId);
            Console.WriteLine(gameBoard.ToString());
            TryDoMove();
        }

        public void Dispose()
        {
            timer.Dispose();
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
            var moveSucceeded = false;
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(50);
                var trainIds = gameBoard
                    .PlayerTrains
                    .Select(pt => pt.Value)
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

                Console.WriteLine($"trying with traingId: {trainIdToPlay}, tileId: {tileIdToPlay}");
                try
                {
                    gameHttpClient.MakeMove(
                        gameId,
                        playerId,
                        trainIdToPlay,
                        tileIdToPlay);
                    i = 100;
                    moveSucceeded = true;
                }
                catch
                {

                }
                Console.WriteLine($"Move succeeded");
            }
            if (!moveSucceeded)
            {
                gameHttpClient.PassMove(
                    gameId,
                    playerId);
            }
            Thread.Sleep(100);
            gameBoard = gameHttpClient
                .GetGameBoard(gameId, playerId);
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