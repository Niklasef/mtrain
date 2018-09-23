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
            //Create game
            MexicanTrainGame.Create(1);
            Console.WriteLine(MexicanTrainGame.Players.First().ToString());

            //Pick a matching tile and place on players train
            var matchingTile = MexicanTrainGame.Players.First().DominoTiles.First(t => t.GetValues().Any(x => x == 12));
            MexicanTrainGame.Players.First().Train.AddTile(matchingTile);
            MexicanTrainGame.Players.First().RemoveTile(matchingTile);
            Console.WriteLine(MexicanTrainGame.Players.First().ToString());
            Console.WriteLine(MexicanTrainGame.Players.First().Train.ToString());

            //Pick another matching tile and place on players train
            matchingTile = MexicanTrainGame
                .Players
                .First()
                .DominoTiles
                .First(t => t
                    .GetValues()
                    .Any(x => MexicanTrainGame
                        .Players
                        .First()
                        .Train
                        .GetTiles()
                        .First()
                        .GetUnlinkedValues()
                        .Any(y => y == x)));
            MexicanTrainGame.Players.First().Train.AddTile(matchingTile);
            MexicanTrainGame.Players.First().RemoveTile(matchingTile);
            Console.WriteLine(MexicanTrainGame.Players.First().ToString());
            Console.WriteLine(MexicanTrainGame.Players.First().Train.ToString());
        }
    }
}
