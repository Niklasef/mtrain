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
            var p1Id = Guid.NewGuid();
            Console.WriteLine($"gId = {gId}");
            Console.WriteLine($"p1Id = {p1Id}");
            gc.CreateGame(gId);
            gc.JoinGame(gId, p1Id, "Niklas");
            var bot = GameBot.Create(gId);
            bot.Start();
            
            Thread.Sleep(20000);
            gc.StartGame(gId);
            bot.Start();


            var gb = gc.GetGameBoard(gId, p1Id);
            Console.WriteLine(gb);
        }
    }
}
