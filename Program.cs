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
            Game.Create(1);
            Console.WriteLine(Game.Players.First().ToString());

            //Pick a matching tile and place on players train
            var matchingTile = Game.Players.First().DominoTiles.First(t => t.GetValues().Any(x => x == 12));
            Game.Players.First().Train.AddTile(matchingTile);
            Game.Players.First().RemoveTile(matchingTile);
            Console.WriteLine(Game.Players.First().ToString());
            Console.WriteLine(Game.Players.First().Train.ToString());

            //Pick another matching tile and place on players train
            matchingTile = Game
                .Players
                .First()
                .DominoTiles
                .First(t => t
                    .GetValues()
                    .Any(x => Game
                        .Players
                        .First()
                        .Train
                        .head
                        .GetUnlinkedValue()
                        .Any(y => y == x)));
            Game.Players.First().Train.AddTile(matchingTile);
            Game.Players.First().RemoveTile(matchingTile);
            Console.WriteLine(Game.Players.First().ToString());
            Console.WriteLine(Game.Players.First().Train.ToString());
        }
    }
}
