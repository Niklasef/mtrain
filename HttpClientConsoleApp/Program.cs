using System;
using System.Linq;
using System.Threading;
using Domain.Game;
using HttpClientBot;

namespace HttpClientConsoleApp
{
    public class Program
    {
        private static GameBoard gameBoard;
        private static HttpClient.GameHttpClient gameClient;
        private static Guid gameId = Guid.NewGuid();
        private static Guid playerId = Guid.NewGuid();

        static void Main(string[] args)
        {
            gameClient = new HttpClient.GameHttpClient(new System.Net.Http.HttpClient());
            gameClient.CreateGame(gameId);
            if (args.FirstOrDefault() != "botonly")
            {
                gameClient.JoinGame(gameId, playerId, "Niklas");
            }
            var bot = GameBot.Create(gameId);
            bot.Start();

            Thread.Sleep(2000);
            gameClient.StartGame(gameId);
            bot.Start();
            bot.OnException += (e) => throw e;
            string input = null;

            gameBoard = gameClient.GetGameBoard(gameId, playerId);
            Console.Clear();
            Console.Write(gameBoard.ToString());

            while (true)
            {
                new Timer((e) =>
                {
                    RefreshGameBoard();
                }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

                if (!TryReadLine(out input, out var tileIndex, out var trainIndex))
                {
                    Console.WriteLine($"Couldn't interpret input: '{input}'. Press any key to continue.");
                    Console.ReadKey();
                    continue;
                }
                if (input?.Equals("p", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    gameClient.PassMove(gameId, playerId);
                    continue;
                }
                if (input?.Equals("q", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    return;
                }
                if (input?.Equals("r", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    gameBoard = gameClient.GetGameBoard(gameId, playerId);
                    Console.Clear();
                    Console.Write(gameBoard.ToString());
                    continue;
                }
                if (tileIndex >= gameBoard.Hand.Count() || tileIndex < 0)
                {
                    Console.WriteLine($"Player doesn't contain tile with index: '{tileIndex}'. Press any key to continue.");
                    Console.ReadKey();
                    continue;
                }
                if (trainIndex > gameBoard.Players.Count() || trainIndex < 0)
                {
                    Console.WriteLine($"Game doesn't contain train with index: '{tileIndex}'. Press any key to continue.");
                    Console.ReadKey();
                    continue;
                }
                var tileId = gameBoard
                    .Hand
                    .Skip(tileIndex)
                    .First()
                    .Id;
                var trainId = trainIndex >= gameBoard.PlayerTrains.Count()
                    ? gameBoard
                        .MexicanTrain
                        .Key
                    : gameBoard
                        .PlayerTrains
                        .Skip(trainIndex)
                        .First()
                        .Key;
                try
                {
                    gameClient.MakeMove(
                        gameId,
                        playerId,
                        trainId,
                        tileId
                    );
                    firstRender = true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Illegal move: '{exception.Message}'. Press any key to continue.");
                    // Console.WriteLine(exception);
                    Console.ReadKey();
                }
            }
        }

        private static bool firstRender = true;
        private static void RefreshGameBoard()
        {
            gameBoard = gameClient.GetGameBoard(gameId, playerId);
            if (gameBoard?.PlayerIdWithTurn == playerId && !firstRender)
            {
                return;
            }
            firstRender = false;
            Console.Clear();
            Console.Write(gameBoard.ToString());
        }

        private static bool TryReadLine(out string input, out int tileIndex, out int trainIndex)
        {
            input = Console.ReadLine();
            if ((input?.Equals("p", StringComparison.OrdinalIgnoreCase) ?? false) ||
                (input?.Equals("q", StringComparison.OrdinalIgnoreCase) ?? false) ||
                (input?.Equals("r", StringComparison.OrdinalIgnoreCase) ?? false))
            {
                tileIndex = -1;
                trainIndex = -1;
                return true;
            }
            if (string.IsNullOrEmpty(input) || input.Split(" ").Count() != 2)
            {
                tileIndex = -1;
                trainIndex = -1;
                return false;
            }
            var inputParts = input.Split(" ");
            if (!int.TryParse(inputParts[0].Trim(), out tileIndex))
            {
                tileIndex = -1;
                trainIndex = -1;
                return false;
            }
            tileIndex--;
            if (!int.TryParse(inputParts[1].Trim(), out trainIndex))
            {
                tileIndex = -1;
                trainIndex = -1;
                return false;
            }
            trainIndex--;
            return true;
        }
    }
}
