using System;
using System.Linq;
using System.Threading;
using Domain.Game;
using HttpClientBot;

namespace HttpClientConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var gc = new HttpClient.GameHttpClient(new System.Net.Http.HttpClient());
            var gId = Guid.NewGuid();
            var pId = Guid.NewGuid();
            Console.WriteLine($"gId = {gId}");
            Console.WriteLine($"p1Id = {pId}");
            gc.CreateGame(gId);
            gc.JoinGame(gId, pId, "Niklas");
            var bot = GameBot.Create(gId);
            bot.Start();

            Thread.Sleep(5000);
            gc.StartGame(gId);
            bot.Start();

            var gb = gc.GetGameBoard(gId, pId);
            Console.Clear();
            Console.WriteLine(gb);
            Thread.Sleep(5000);

            gc.MakeMove(gId, pId, gb.MexicanTrain.Key, gb.Hand.First().Id);
            Thread.Sleep(1000);
            gb = gc.GetGameBoard(gId, pId);
            Console.Clear();
            Console.WriteLine(gb);
            Thread.Sleep(30000);

            gb = gc.GetGameBoard(gId, pId);
            Console.Clear();
            Console.WriteLine(gb);
            Thread.Sleep(5000);
        }
    }
}
