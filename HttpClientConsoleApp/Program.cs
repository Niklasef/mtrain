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
        private static Guid gameId = Guid.Parse("2ec86ee1-e181-4b1c-ab87-73a7a87c1e28");
        private static Guid playerId = Guid.NewGuid();

        static void Main(string[] args)
        {
            var botOnly = args.FirstOrDefault() == "bot";
            gameClient = new HttpClient.GameHttpClient(new System.Net.Http.HttpClient());
            if (!botOnly)
            {
                Console.WriteLine("Human player mode");
                var gameBoards = gameClient.GetGameBoards();
                if (!gameBoards.Any(gb => gb.GameId == gameId))
                {
                    Console.WriteLine("Creating game");
                    gameClient.CreateGame(gameId);
                }
                gameClient.JoinGame(gameId, playerId, "Niklas");
                Console.WriteLine("Waiting for bot...");
                Thread.Sleep(10000);
                Console.WriteLine("Starting game");
                gameClient.StartGame(gameId);
            }
            else
            {
                var bot = GameBot.Create(gameId);
                bot.OnException += (e) => throw e;
                bot.Start();
            }

            if (!botOnly)
            {
                RunHumanPlayer();
            }
            else
            {
                while (true)
                {
                    Thread.Sleep(5000);
                }
            }
        }

        private static void RunHumanPlayer()
        {
            string input = null;
            gameBoard = gameClient.GetGameBoard(gameId, playerId);
            Console.Write(gameBoard.ToString());

            while (true)
            {
                if (!TryReadLine(out input, out var tileIndex, out var trainIndex))
                {
                    Console.WriteLine($"Couldn't interpret input: '{input}'. Press any key to continue. (legal: p, q, r [ix ix])");
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
                        .Value;

                Console.WriteLine($"tileIndex {tileIndex}");
                Console.WriteLine($"tileId {tileId}");
                Console.WriteLine($"trainIndex {trainIndex}");
                Console.WriteLine($"trainId {trainId}");
                try
                {
                    gameClient.MakeMove(
                        gameId,
                        playerId,
                        trainId,
                        tileId
                    );
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Exception when making move.");
                    Console.ReadKey();
                }
            }
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
