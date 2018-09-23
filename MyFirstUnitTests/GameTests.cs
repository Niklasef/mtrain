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
            MexicanTrainGame.Create(2);
            var boneyard = MexicanTrainGame.Boneyard;
            Assert.Equal(
                70,
                boneyard.Count());
        }

        [Fact]
        public void CreatingGame_TwoPlayers_10TilesEach()
        {
            MexicanTrainGame.Create(2);
            Assert.Equal(
                10,
                MexicanTrainGame.Players.First().DominoTiles.Count());
            Assert.Equal(
                10,
                MexicanTrainGame.Players.Last().DominoTiles.Count());
        }
    }
}
