using System;
using Xunit;
using Domain;
using Domain.Game;
using Domain.Player;
using Domain.DominoTile;
using Domain.Train;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = MexicanTrainGame
                .Create(new[] { "Johannes", "Niklas" }.ToHashSet());
            string input = null;

            while (true)
            {
                Console.Clear();
                Console.WriteLine(game.ToString());

                var playerWithTurn = game
                    .Players
                    .First(p => p.GetStateType().Name.Equals("HasTurnState"));

                if (!TryReadLine(out input, out var tileIndex, out var trainIndex))
                {
                    Console.WriteLine($"Couldn't interpret input: '{input}'. Press any key to continue.");
                    Console.ReadKey();
                    continue;
                }
                if (input?.Equals("p", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    game.PassMove(playerWithTurn.Id);
                    continue;
                }
                if (input?.Equals("q", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    return;
                }
                if (tileIndex >= playerWithTurn.Hand.Count() || tileIndex < 0)
                {
                    Console.WriteLine($"Player doesn't contain tile with index: '{tileIndex}'. Press any key to continue.");
                    Console.ReadKey();
                    continue;
                }
                if (trainIndex > game.Players.Count() || trainIndex < 0)
                {
                    Console.WriteLine($"Game doesn't contain train with index: '{tileIndex}'. Press any key to continue.");
                    Console.ReadKey();
                    continue;
                }
                var tileId = playerWithTurn
                    .Hand
                    .Skip(tileIndex)
                    .First()
                    .Id;
                var trainId = trainIndex >= game.Players.Count()
                    ? game
                        .MexicanTrain
                        .Id
                    : game
                        .Players
                        .Skip(trainIndex)
                        .First()
                        .Train
                        .Id;
                try
                {
                    game.MakeMove(
                        playerWithTurn.Id,
                        tileId,
                        trainId
                    );
                }
                catch (ApplicationException exception)
                {
                    Console.WriteLine($"Illegal move: '{exception.Message}'. Press any key to continue.");
                    Console.ReadKey();
                }
            }
        }

        private static bool TryReadLine(out string input, out int tileIndex, out int trainIndex)
        {
            input = Console.ReadLine();
            if (input?.Equals("p", StringComparison.OrdinalIgnoreCase) ?? false)
            {
                tileIndex = -1;
                trainIndex = -1;
                return true;
            }
            if (input?.Equals("q", StringComparison.OrdinalIgnoreCase) ?? false)
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
