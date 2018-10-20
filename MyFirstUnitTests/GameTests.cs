using Xunit;
using Domain;
using System.Linq;
using System;

namespace MyFirstUnitTests
{
    public class GameTests
    {
        [Fact]
        public void MakeMove_WithTileMatchingPlayersTrain_PlayersTrainHasNewTile()
        {
            var gameId = Guid.NewGuid();
            var mexicanTrain = new MexicanTrain();
            var playedTile = new DominoTile(11, 12);
            var player = new Player(gameId, "test", (new[] { playedTile }).ToHashSet());
            player.GiveTurn();
            var engine = new DominoTile(12, 12);
            engine.State = new EngineState();
            var sut = new MexicanTrainGame(
                gameId,
                new[] { player },
                mexicanTrain,
                engine,
                Enumerable.Empty<DominoTile>().ToList());
            Games.Add(sut.Id, sut);

            sut.MakeMove(player.Id, playedTile.Id, player.Train.Id);

            Assert.Equal(playedTile, sut.Players.First().Train.GetTiles().First());
        }

        [Fact]
        public void MakeMove_PlayerWithTurnPutsTileOnMexicanTrain_MexicanTrainHasNewTile()
        {
            var gameId = Guid.NewGuid();
            var mexicanTrain = new MexicanTrain();
            var playedTile = new DominoTile(11, 12);
            var player = new Player(gameId, "test", (new[] { playedTile }).ToHashSet());
            player.GiveTurn();
            var engine = new DominoTile(12, 12);
            engine.State = new EngineState();
            var sut = new MexicanTrainGame(
                gameId,
                new[] { player },
                mexicanTrain,
                engine,
                Enumerable.Empty<DominoTile>().ToList());
            Games.Add(sut.Id, sut);

            sut.MakeMove(player.Id, playedTile.Id, mexicanTrain.Id);

            Assert.Equal(playedTile, sut.MexicanTrain.GetTiles().First());
        }

        [Fact]
        public void PassMove_WithTwoPlayers_PlayerIsGivenNewTileAndTrainIsOpen()
        {
            var gameId = Guid.NewGuid();
            var mexicanTrain = new MexicanTrain();
            var playedTile = new DominoTile(11, 12);
            var firstPlayer = new Player(gameId, "playerOne", (new[] { playedTile }).ToHashSet());
            var secondPlayer = new Player(gameId, "playerTwo", (new[] { playedTile }).ToHashSet());
            firstPlayer.GiveTurn();
            var engine = new DominoTile(12, 12);
            engine.State = new EngineState();
            var sut = new MexicanTrainGame(
                gameId,
                new[] { firstPlayer },
                mexicanTrain,
                engine,
                new[] { new DominoTile(5, 6) }.ToList());
            Games.Add(sut.Id, sut);

            sut.PassMove(firstPlayer.Id);

            Assert.Equal(2, firstPlayer.Hand.Count);
            Assert.Equal(typeof(PlayerTrain.OpenPlayerTrainState), ((PlayerTrain)firstPlayer.Train).state.GetType());
        }

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
