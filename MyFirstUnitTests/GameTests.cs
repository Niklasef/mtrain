using Xunit;
using Domain;
using System.Linq;

namespace MyFirstUnitTests
{
    public class GameTests
    {
        [Fact]
        public void CreatingGame_TwoPlayers_70TilesInBoneyard()
        {
            var boneyard = new Game(new[] { new Player(), new Player() })
                .Boneyard;
            Assert.Equal(
                70,
                boneyard.Count());
        }

        [Fact]
        public void CreatingGame_TwoPlayers_10TilesEach()
        {
            var game = new Game(new[] { new Player(), new Player() });
            Assert.Equal(
                10,
                game.Players.First().dominoTiles.Count());
            Assert.Equal(
                10,
                game.Players.Last().dominoTiles.Count());
        }
    }
}
