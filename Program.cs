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
            MexicanTrainGame.Create(new[] { "1", "2" }.ToHashSet());
            Console.WriteLine(MexicanTrainGame.Players.First().ToString());
        }
    }
}
