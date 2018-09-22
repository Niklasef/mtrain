using Xunit;
using Domain;
using System.Linq;

namespace MyFirstUnitTests
{
    public class PlayerTrainTests
    {
        [Fact]
        public void AddTile_ThreeTiles_FourTilesIncludingEngineExists()
        {
            var engine = new DominoTile(12, 12);
            engine.State = new EngineState();
            var sut = new PlayerTrain(engine);
            var tileOne = new DominoTile(12, 11);
            var tileTwo = new DominoTile(11, 10);
            var tileThree = new DominoTile(10, 9);

            sut.Open();
            sut.AddTile(tileOne);
            sut.AddTile(tileTwo);
            sut.AddTile(tileThree);

            Assert.Equal(tileThree, sut.GetTiles().First());
            Assert.Equal(tileTwo, sut.GetTiles().First().LinkedTile);
            Assert.Equal(tileOne, sut.GetTiles().First().LinkedTile.LinkedTile);
            Assert.Equal(engine, sut.GetTiles().First().LinkedTile.LinkedTile.LinkedTile);
            Assert.Equal(null, sut.GetTiles().First().LinkedTile.LinkedTile.LinkedTile.LinkedTile);
        }

        [Fact]
        public void GetTiles_ThreeTilesInTrain_CorrectTilesReceived()
        {
            var engine = new DominoTile(12, 12);
            engine.State = new EngineState();
            var sut = new PlayerTrain(engine);
            var tileOne = new DominoTile(12, 11);
            var tileTwo = new DominoTile(11, 10);
            sut.Open();
            sut.AddTile(tileOne);
            sut.AddTile(tileTwo);

            var result = sut.GetTiles();

            Assert.Equal(tileTwo, result.First());
            Assert.Equal(tileOne, result.Skip(1).First());
            Assert.Equal(engine, result.Skip(2).First());
        }
    }
}
