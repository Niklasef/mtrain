using System;
using Xunit;
using Domain;
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
                input = Console.ReadLine();
                var playerWithTurn = game
                    .Players
                    .First(p => p.GetStateType().Name.Equals("HasTurnState"));
                if (input?.Equals("p", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    game.PassMove(playerWithTurn.Id);
                    continue;
                }
                if (input?.Equals("q", StringComparison.OrdinalIgnoreCase) ?? false)
                {
                    return;
                }
                var inputParts = input.Split(" ");
                var tileIndex = int.Parse(inputParts[0].Trim()) - 1;
                var trainIndex = int.Parse(inputParts[1].Trim()) - 1;
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
                game.MakeMove(
                    playerWithTurn.Id,
                    tileId,
                    trainId
                );
            }
        }
    }
}
