using System;
using System.Linq;
using Domain.Game;

namespace HttpClientConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var gc = new HttpClient.GameHttpClient(new System.Net.Http.HttpClient());
            var gId = Guid.NewGuid();
            var p1Id = Guid.NewGuid();
            var p2Id = Guid.NewGuid();
            Console.WriteLine($"gId = {gId}");
            Console.WriteLine($"p1Id = {p1Id}");
            gc.CreateGame(gId);
            gc.JoinGame(gId, p1Id, "Niklas");
            gc.JoinGame(gId, p2Id, "Johannes");
            gc.StartGame(gId);
            var gb = gc.GetGameBoard(gId, p1Id);
            Console.WriteLine(gb);
            gc.PassMove(gId, p1Id);
            gb = gc.GetGameBoard(gId, p2Id);
            Console.WriteLine(gb);
        }
    }
}
