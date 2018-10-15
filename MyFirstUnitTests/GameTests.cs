using Xunit;
using Domain;
using System.Linq;
using System;

namespace MyFirstUnitTests
{
    public class GameTests
    {
        // [Fact]
        // public void IsLegalMove_NoMatchingTileOnMexicanTrain_False()
        // {
        //     var gameId = Guid.NewGuid();
        //     var mexicanTrain = new MexicanTrain();
        //     var player = new Player(gameId, "test", (new[] { new DominoTile(11, 11) }).ToHashSet());
        //     mexicanTrain.AddTile(new DominoTile(10, 10), player.Id);
        //     var sut = new MexicanTrainGame(
        //         Guid.NewGuid(),
        //         new[] { player },
        //         mexicanTrain,
        //         new DominoTile(12, 12),
        //         Enumerable.Empty<DominoTile>().ToList());

        //     var result = sut.IsLegalMove(sut.Players.First().Id, new DominoTile(11, 11), sut.MexicanTrain.Id);

        //     Assert.Equal(false, false);
        //     Games.Remove(sut.Id);
        // }

        [Fact]
        public void CreatingGame_TwoPlayers_70TilesInBoneyard()
        {
            var sut = MexicanTrainGame.Create(new[] { "1", "2" }.ToHashSet());
            var boneyard = sut.Boneyard;
            Assert.Equal(
                70,
                boneyard.Count());
        }

        [Fact]
        public void CreatingGame_TwoPlayers_10TilesEach()
        {
            var sut = MexicanTrainGame.Create(new[] { "1", "2" }.ToHashSet());
            Assert.Equal(
                10,
                sut.Players.First().Hand.Count());
            Assert.Equal(
                10,
                sut.Players.Last().Hand.Count());
        }
    }
}
