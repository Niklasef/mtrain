using Xunit;
using Domain;
using Domain.Player;
using Domain.Game;
using Domain.DominoTile;
using Domain.Train;
using System.Linq;
using System;
using System.Collections.Generic;

namespace MyFirstUnitTests
{
    public class GameTests
    {
        [Fact]
        public void MakeMove_WithTileMatchingPlayersTrain_PlayersTrainHasNewTileAndTurnIsPassad()
        {
            var playedTile = new DominoTileEntity(11, 12);
            var sut = CreateGame(playedTile);

            sut.MakeMove(sut.Players.First().Id, playedTile.Id, sut.Players.First().Train.Id);

            Assert.Equal(playedTile, sut.Players.First().Train.GetTiles().First());
            Assert.Equal(typeof(PlayerEntity.WaitingForTurnState), sut.Players.First().GetStateType());
            Assert.Equal(typeof(PlayerEntity.HasTurnState), sut.Players.Skip(1).First().GetStateType());
        }

        [Fact]
        public void MakeMove_WithLastPlayer_FirstPlayerIsGivenTurn()
        {
            var firstPlayedTiles = new[] { new DominoTileEntity(11, 12), new DominoTileEntity(1, 2) };
            var secondPlayedTiles = new[] { new DominoTileEntity(10, 12), new DominoTileEntity(3, 2) };
            var sut = CreateGame(firstPlayedTiles, secondPlayedTiles);

            sut.MakeMove(sut.Players.First().Id, firstPlayedTiles.First().Id, sut.Players.First().Train.Id);
            sut.MakeMove(sut.Players.Last().Id, secondPlayedTiles.First().Id, sut.Players.Last().Train.Id);

            Assert.Equal(typeof(PlayerEntity.HasTurnState), sut.Players.First().GetStateType());
            Assert.Equal(typeof(PlayerEntity.WaitingForTurnState), sut.Players.Last().GetStateType());
        }

        [Fact]
        public void MakeMove_WithDoubleOnMexTrain_TurnIsNotPassed()
        {
            var playedTile = new DominoTileEntity(11, 11);
            var sut = CreateGame(playedTile);

            sut.MakeMove(
                sut.Players.First().Id,
                playedTile.Id,
                sut.MexicanTrain.Id);

            Assert.Equal(typeof(PlayerEntity.HasTurnState), sut.Players.First().GetStateType());
            Assert.Equal(typeof(PlayerEntity.WaitingForTurnState), sut.Players.Skip(1).First().GetStateType());
        }

        [Fact]
        public void MakeMove_WithDoubleOnMexTrainAndSecondFreeMoveIsMade_TurnIsPassed()
        {
            var playedTiles = new[] { new DominoTileEntity(11, 11), new DominoTileEntity(12, 11) };
            var sut = CreateGame(playedTiles, new[] { new DominoTileEntity(2, 1) });

            sut.MakeMove(
                sut.Players.First().Id,
                playedTiles.First().Id,
                sut.MexicanTrain.Id);
            sut.MakeMove(
                sut.Players.First().Id,
                playedTiles.Last().Id,
                sut.Players.First().Train.Id);

            Assert.Equal(typeof(PlayerEntity.WaitingForTurnState), sut.Players.First().GetStateType());
            Assert.Equal(typeof(PlayerEntity.HasTurnState), sut.Players.Skip(1).First().GetStateType());
        }

        [Fact]
        public void MakeMove_DoublePlayedAndOherPlayerDoesntPlayOnDouble_IllegalTurn()
        {
            var firstPlayerTiles = new[] { new DominoTileEntity(11, 11), new DominoTileEntity(12, 11) };
            var secondPlayerTiles = new[] { new DominoTileEntity(11, 10), new DominoTileEntity(12, 10) };
            var sut = CreateGame(firstPlayerTiles, secondPlayerTiles);

            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.First().Id,
                sut.MexicanTrain.Id);
            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.Last().Id,
                sut.Players.Last().Train.Id);
            Action illegalPlay = () => sut.MakeMove(
                sut.Players.Last().Id,
                secondPlayerTiles.Last().Id,
                sut.Players.Last().Train.Id);

            Assert.ThrowsAny<ApplicationException>(illegalPlay);
        }

        [Fact]
        public void MakeMove_DoublePlayedAndThenPass_OtherPlayersTurn()
        {
            var firstPlayerTiles = new[] { new DominoTileEntity(11, 11), new DominoTileEntity(12, 11) };
            var secondPlayerTiles = new[] { new DominoTileEntity(11, 10), new DominoTileEntity(12, 10) };
            var sut = CreateGame(firstPlayerTiles, secondPlayerTiles);

            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.First().Id,
                sut.MexicanTrain.Id);
            sut.PassMove(
                sut.Players.First().Id);

            Assert.Equal("HasTurnState", sut.Players.Last().GetStateType().Name);
        }

        [Fact]
        public void MakeMove_DoublePlayedAndOherPlayerPlayOnDouble_GameStateIsNoDoubles()
        {
            var firstPlayerTiles = new[] { new DominoTileEntity(11, 11), new DominoTileEntity(12, 11) };
            var secondPlayerTiles = new[] { new DominoTileEntity(11, 10), new DominoTileEntity(12, 10) };
            var sut = CreateGame(firstPlayerTiles, secondPlayerTiles);

            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.First().Id,
                sut.MexicanTrain.Id);
            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.Last().Id,
                sut.Players.Last().Train.Id);
            sut.MakeMove(
                sut.Players.Last().Id,
                secondPlayerTiles.First().Id,
                sut.MexicanTrain.Id);

            Assert.Equal("NoDoublesGameState", sut.GetStateType().Name);
        }

        [Fact]
        public void MakeMove_OpenAndClosePlayersTrains_PlayersTrainIsClosed()
        {
            var firstPlayerTiles = new[] { new DominoTileEntity(12, 11), new DominoTileEntity(11, 10), new DominoTileEntity(2, 1) };
            var secondPlayerTiles = new[] { new DominoTileEntity(12, 10), new DominoTileEntity(2, 3) };
            var sut = CreateGame(firstPlayerTiles, secondPlayerTiles);

            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.First().Id,
                sut.Players.First().Train.Id);
            sut.PassMove(
                sut.Players.Last().Id);
            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.Skip(1).First().Id,
                sut.Players.First().Train.Id);
            sut.MakeMove(
                sut.Players.Last().Id,
                secondPlayerTiles.First().Id,
                sut.Players.Last().Train.Id);

            Assert.Equal("ClosedPlayerTrainState", sut.Players.Last().Train.GetStateType().Name);
        }

        [Fact]
        public void MakeMove_PlayerWithTurnPutsTileOnMexicanTrain_MexicanTrainHasNewTile()
        {
            var playedTile = new DominoTileEntity(11, 12);
            var sut = CreateGame(playedTile);

            sut.MakeMove(sut.Players.First().Id, playedTile.Id, sut.MexicanTrain.Id);

            Assert.Equal(playedTile, sut.MexicanTrain.GetTiles().First());
        }

        [Fact]
        public void MakeMove_PlayerWithTurnPutsTileOnClosedPlayerTrain_Exception()
        {
            var playedTile = new DominoTileEntity(11, 12);
            var sut = CreateGame(playedTile);

            Action makeMove = () => sut.MakeMove(
                sut.Players.First().Id,
                playedTile.Id,
                sut.Players.Skip(1).First().Train.Id);

            Assert.ThrowsAny<Exception>(makeMove);
        }

        [Fact]
        public void PassAndMove_PlayerWithTurnPutsTileOnOtherPlayersOpenTrain_TileIsMoved()
        {
            var playedTile = new DominoTileEntity(11, 12);
            var sut = CreateGame(playedTile);

            sut.PassMove(sut.Players.First().Id);
            sut.PassMove(sut.Players.Skip(1).First().Id);
            sut.MakeMove(
                sut.Players.First().Id,
                playedTile.Id,
                sut.Players.Skip(1).First().Train.Id);

            Assert.Equal(playedTile, sut.Players.Skip(1).First().Train.GetTiles().First());
        }

        [Fact]
        public void PassMove_WithTwoPlayers_PlayerIsGivenNewTileAndTrainIsOpenAndTurnIsPassed()
        {
            var sut = CreateGame(new DominoTileEntity(11, 12));

            sut.PassMove(sut.Players.First().Id);

            Assert.Equal(2, sut.Players.First().Hand.Count);
            Assert.Equal(typeof(PlayerTrain.OpenPlayerTrainState), ((PlayerTrain)sut.Players.First().Train).state.GetType());
            Assert.Equal(typeof(PlayerEntity.WaitingForTurnState), sut.Players.First().GetStateType());
            Assert.Equal(typeof(PlayerEntity.HasTurnState), sut.Players.Skip(1).First().GetStateType());
        }

        [Fact]
        public void PassMoveTwice_WithTwoPlayers_FirstPlayerHasTurn()
        {
            var sut = CreateGame(new DominoTileEntity(11, 12));

            sut.PassMove(sut.Players.First().Id);
            sut.PassMove(sut.Players.Skip(1).First().Id);

            Assert.Equal(typeof(PlayerEntity.WaitingForTurnState), sut.Players.Skip(1).First().GetStateType());
            Assert.Equal(typeof(PlayerEntity.HasTurnState), sut.Players.First().GetStateType());
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
        private static MexicanTrainGame CreateGame(DominoTileEntity firstPlayedTile)
        {
            return CreateGame(firstPlayedTile, new DominoTileEntity(5, 7));
        }
        private static MexicanTrainGame CreateGame(DominoTileEntity firstPlayedTile, DominoTileEntity secondPlayedTile)
        {
            return CreateGame(new[] { firstPlayedTile }, new[] { secondPlayedTile });
        }
        private static MexicanTrainGame CreateGame(IEnumerable<DominoTileEntity> firstPlayedTiles, IEnumerable<DominoTileEntity> secondPlayedTiles)
        {
            var gameId = Guid.NewGuid();
            var mexicanTrain = new MexicanTrain();
            var firstPlayer = new PlayerEntity(gameId, "playerOne", firstPlayedTiles.ToHashSet());
            var secondPlayer = new PlayerEntity(gameId, "playerTwo", secondPlayedTiles.ToHashSet());
            firstPlayer.GiveTurn();
            var engine = new DominoTileEntity(12, 12, true);
            var sut = new MexicanTrainGame(
                gameId,
                new[] { firstPlayer, secondPlayer },
                mexicanTrain,
                engine,
                new[] { new DominoTileEntity(9, 8), new DominoTileEntity(9, 9) }.ToList());
            Games.Add(sut.Id, sut);
            return sut;
        }
    }
}
