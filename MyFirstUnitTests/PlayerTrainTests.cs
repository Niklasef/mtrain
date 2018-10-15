using Xunit;
using Domain;
using System.Linq;
using System;

namespace MyFirstUnitTests
{

    public class PlayerTrainTests
    {
        //TODO: fix test
        // [Fact]
        // public void AddTile_ThreeTiles_FourTilesIncludingEngineExists()
        // {
        //     var engine = new DominoTile(12, 12);
        //     engine.State = new EngineState();
        //     var playerId = Guid.NewGuid();
        //     var sut = new PlayerTrain(engine, playerId);
        //     var tileOne = new DominoTile(12, 11);
        //     var tileTwo = new DominoTile(11, 10);
        //     var tileThree = new DominoTile(10, 9);

        //     sut.Open();
        //     sut.AddTile(tileOne, playerId);
        //     sut.AddTile(tileTwo, playerId);
        //     sut.AddTile(tileThree, playerId);

        //     Assert.Equal(tileThree, sut.GetTiles().First());
        //     Assert.Equal(tileTwo, sut.GetTiles().First().LinkedTiles);
        //     Assert.Equal(tileOne, sut.GetTiles().First().LinkedTiles.LinkedTile);
        //     Assert.Equal(engine, sut.GetTiles().First().LinkedTiles.LinkedTile.LinkedTile);
        //     Assert.Equal(null, sut.GetTiles().First().LinkedTiles.LinkedTile.LinkedTile.LinkedTile);
        // }

        // [Fact]
        // public void GetTiles_ThreeTilesInTrain_CorrectTilesReceived()
        // {
        //     var engine = new DominoTile(12, 12);
        //     engine.State = new EngineState();
        //     var playerId = Guid.NewGuid();
        //     var sut = new PlayerTrain(engine, playerId);
        //     var tileOne = new DominoTile(12, 11);
        //     var tileTwo = new DominoTile(11, 10);
        //     sut.Open();
        //     sut.AddTile(tileOne, playerId);
        //     sut.AddTile(tileTwo, playerId);

        //     var result = sut.GetTiles();

        //     Assert.Equal(tileTwo, result.First());
        //     Assert.Equal(tileOne, result.Skip(1).First());
        //     Assert.Equal(engine, result.Skip(2).First());
        // }
    }
}
