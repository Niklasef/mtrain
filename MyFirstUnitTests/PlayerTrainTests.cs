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

            sut.AddTile(tileOne);
            sut.AddTile(tileTwo);
            sut.AddTile(tileThree);

            Assert.Equal(tileThree, sut.head);
            Assert.Equal(tileTwo, sut.head.LinkedTile);
            Assert.Equal(tileOne, sut.head.LinkedTile.LinkedTile);
            Assert.Equal(engine, sut.head.LinkedTile.LinkedTile.LinkedTile);
            Assert.Equal(null, sut.head.LinkedTile.LinkedTile.LinkedTile.LinkedTile);
        }

        [Fact]
        public void GetTiles_ThreeTilesInTrain_CorrectTilesReceived()
        {
            var engine = new DominoTile(12, 12);
            engine.State = new EngineState();
            var sut = new PlayerTrain(engine);
            var tileOne = new DominoTile(12, 11);
            var tileTwo = new DominoTile(11, 10);
            sut.AddTile(tileOne);
            sut.AddTile(tileTwo);

            var result = sut.GetTiles();

            Assert.Equal(tileTwo, result.First());
            Assert.Equal(tileOne, result.Skip(1).First());
            Assert.Equal(engine, result.Skip(2).First());
        }
    }
}
