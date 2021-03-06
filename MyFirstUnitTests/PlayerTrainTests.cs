using Xunit;
using Domain;
using Domain.DominoTile;
using Domain.Player;
using Domain.Game;
using Domain.Train;
using System.Linq;
using System;
using System.Collections.Generic;

namespace MyFirstUnitTests
{
    public class PlayerTrainTests
    {
        [Fact]
        public void AddTile_ThreeTiles_FourTilesIncludingEngineExists()
        {
            var engine = new DominoTileEntity(12, 12, true);
            var game = new GameEntity(
                Guid.NewGuid(), 
                Enumerable.Empty<PlayerEntity>(),
                MexicanTrain.Create(),
                engine,
                new Stack<DominoTileEntity>());
            Games.Add(game.Id, game);

            var playerId = Guid.NewGuid();
            var sut = new PlayerTrain(engine, playerId);
            var tileOne = new DominoTileEntity(12, 11);
            var tileTwo = new DominoTileEntity(11, 10);
            var tileThree = new DominoTileEntity(10, 9);

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
            var engine = new DominoTileEntity(12, 12, true);
            var game = new GameEntity(
                Guid.NewGuid(), 
                Enumerable.Empty<PlayerEntity>(),
                MexicanTrain.Create(),
                engine,
                new Stack<DominoTileEntity>());
            Games.Add(game.Id, game);

            var playerId = Guid.NewGuid();
            var sut = new PlayerTrain(engine, playerId);
            var tileOne = new DominoTileEntity(12, 11);
            var tileTwo = new DominoTileEntity(11, 10);
            sut.Open();
            sut.AddTile(tileOne, playerId);
            sut.AddTile(tileTwo, playerId);

            var result = sut.GetTiles();

            Assert.Equal(tileTwo, result.First());
            Assert.Equal(tileOne, result.Skip(1).First());
            Assert.Equal(engine, result.Skip(2).First());
        }

        [Fact]
        public void ToString_ThreeTilesAddes_AlignedTiles()
        {
            var engine = new DominoTileEntity(12, 12, true);
            var game = new GameEntity(
                Guid.NewGuid(), 
                Enumerable.Empty<PlayerEntity>(),
                MexicanTrain.Create(),
                engine,
                new Stack<DominoTileEntity>());
            Games.Add(game.Id, game);

            var playerId = Guid.NewGuid();
            var sut = new PlayerTrain(engine, playerId);
            var tileOne = new DominoTileEntity(11, 12);
            var tileTwo = new DominoTileEntity(10, 11);
            sut.Open();
            sut.AddTile(tileOne, playerId);
            sut.AddTile(tileTwo, playerId);

            var result = sut.ToString();

            Assert.Equal("[12|12], [12|11], [11|10]", result);
        }

        [Fact]
        public void AddTileNonMatchingTile_WhenOpen_ExceptionThrown()
        {
            var engine = new DominoTileEntity(12, 12, true);
            var game = new GameEntity(
                Guid.NewGuid(), 
                Enumerable.Empty<PlayerEntity>(),
                MexicanTrain.Create(),
                engine,
                new Stack<DominoTileEntity>());
            Games.Add(game.Id, game);

            var playerId = Guid.NewGuid();
            var sut = new PlayerTrain(engine, playerId);
            var tileOne = new DominoTileEntity(12, 11);
            var tileTwo = new DominoTileEntity(11, 10);
            var tileThree = new DominoTileEntity(11, 3);
            sut.AddTile(tileOne, playerId);
            sut.Open();
            sut.AddTile(tileTwo, playerId);
            Action addTile = () => sut.AddTile(tileThree, playerId);

            Assert.ThrowsAny<Exception>(addTile);
        }
    }
}
