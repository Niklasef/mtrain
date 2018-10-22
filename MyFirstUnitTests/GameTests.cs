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
            var playedTile = new DominoTile(11, 12);
            var sut = CreateGame(playedTile);

            sut.MakeMove(sut.Players.First().Id, playedTile.Id, sut.Players.First().Train.Id);

            Assert.Equal(playedTile, sut.Players.First().Train.GetTiles().First());
        }

        private static MexicanTrainGame CreateGame(DominoTile playedTile)
        {
            var gameId = Guid.NewGuid();
            var mexicanTrain = new MexicanTrain();
            var firstPlayer = new Player(gameId, "playerOne", (new[] { playedTile }).ToHashSet());
            var secondPlayer = new Player(gameId, "playerTwo", (new[] { new DominoTile(5, 7) }).ToHashSet());
            firstPlayer.GiveTurn();
            var engine = new DominoTile(12, 12);
            engine.State = new EngineState();
            var sut = new MexicanTrainGame(
                gameId,
                new[] { firstPlayer, secondPlayer },
                mexicanTrain,
                engine,
                new[] { new DominoTile(9, 8) }.ToList());
            Games.Add(sut.Id, sut);
            return sut;
        }

        [Fact]
        public void MakeMove_PlayerWithTurnPutsTileOnMexicanTrain_MexicanTrainHasNewTile()
        {
            var playedTile = new DominoTile(11, 12);
            var sut = CreateGame(playedTile);

            sut.MakeMove(sut.Players.First().Id, playedTile.Id, sut.MexicanTrain.Id);

            Assert.Equal(playedTile, sut.MexicanTrain.GetTiles().First());
        }

        [Fact]
        public void PassMove_WithTwoPlayers_PlayerIsGivenNewTileAndTrainIsOpenAndTurnIsPassed()
        {
            var playedTile = new DominoTile(11, 12);
            var sut = CreateGame(playedTile);

            sut.PassMove(sut.Players.First().Id);

            Assert.Equal(2, sut.Players.First().Hand.Count);
            Assert.Equal(typeof(PlayerTrain.OpenPlayerTrainState), ((PlayerTrain)sut.Players.First().Train).state.GetType());
            Assert.Equal(typeof(Player.WaitingForTurnState), sut.Players.First().GetStateType());
            Assert.Equal(typeof(Player.HasTurnState), sut.Players.Skip(1).First().GetStateType());
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
