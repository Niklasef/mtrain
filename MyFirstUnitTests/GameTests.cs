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
            MexicanTrainGame.Create(new [] {"1", "2"}.ToHashSet());
            var boneyard = MexicanTrainGame.Boneyard;
            Assert.Equal(
                70,
                boneyard.Count());
        }

        [Fact]
        public void CreatingGame_TwoPlayers_10TilesEach()
        {
            MexicanTrainGame.Create(new [] {"1", "2"}.ToHashSet());
            Assert.Equal(
                10,
                MexicanTrainGame.Players.First().Hand.Count());
            Assert.Equal(
                10,
                MexicanTrainGame.Players.Last().Hand.Count());
        }
    }
}
