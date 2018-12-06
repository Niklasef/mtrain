using Xunit;
using Domain;
using System.Linq;
using System;

namespace MyFirstUnitTests
{

    public class PlayerTrainTests
    {
        [Fact]
        public void AddTile_ThreeTiles_FourTilesIncludingEngineExists()
        {
            var engine = new DominoTile(12, 12);
            engine.State = new EngineState();
            var game = new MexicanTrainGame(
                Guid.NewGuid(), 
                Enumerable.Empty<Player>(),
                new MexicanTrain(),
                engine,
                Enumerable.Empty<DominoTile>().ToArray());
            Games.Add(game.Id, game);

            var playerId = Guid.NewGuid();
            var sut = new PlayerTrain(game.Id, playerId);
            var tileOne = new DominoTile(12, 11);
            var tileTwo = new DominoTile(11, 10);
            var tileThree = new DominoTile(10, 9);

            sut.Open();
            sut.AddTile(tileOne, playerId);
            sut.AddTile(tileTwo, playerId);
            sut.AddTile(tileThree, playerId);

            Assert.Equal(tileThree, sut.GetTiles().First());
            Assert.True(sut.GetTiles().First().IsLinked(tileTwo));
            Assert.True(sut.GetTiles().Skip(1).First().IsLinked(tileOne));
            Assert.True(sut.GetTiles().Skip(2).First().IsLinked(engine));
            Assert.Equal(4, sut.GetTiles().Count());
        }

        [Fact]
        public void GetTiles_ThreeTilesInTrain_CorrectTilesReceived()
        {
            var engine = new DominoTile(12, 12);
            engine.State = new EngineState();
            var game = new MexicanTrainGame(
                Guid.NewGuid(), 
                Enumerable.Empty<Player>(),
                new MexicanTrain(),
                engine,
                Enumerable.Empty<DominoTile>().ToArray());
            Games.Add(game.Id, game);

            var playerId = Guid.NewGuid();
            var sut = new PlayerTrain(game.Id, playerId);
            var tileOne = new DominoTile(12, 11);
            var tileTwo = new DominoTile(11, 10);
            sut.Open();
            sut.AddTile(tileOne, playerId);
            sut.AddTile(tileTwo, playerId);

            var result = sut.GetTiles();

            Assert.Equal(tileTwo, result.First());
            Assert.Equal(tileOne, result.Skip(1).First());
            Assert.Equal(engine, result.Skip(2).First());
        }
    }
}
