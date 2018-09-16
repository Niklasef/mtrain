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
            Game.Create(2);
            var boneyard = Game.Boneyard;
            Assert.Equal(
                70,
                boneyard.Count());
        }

        [Fact]
        public void CreatingGame_TwoPlayers_10TilesEach()
        {
            Game.Create(2);
            Assert.Equal(
                10,
                Game.Players.First().DominoTiles.Count());
            Assert.Equal(
                10,
                Game.Players.Last().DominoTiles.Count());
        }
    }
}
