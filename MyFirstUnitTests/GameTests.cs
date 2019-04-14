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
            Assert.Equal("WaitingForTurnPlayerState", sut.Players.First().GetStateType().Name);
            Assert.Equal("HasTurnPlayerState", sut.Players.Skip(1).First().GetStateType().Name);
        }

        [Fact]
        public void MakeMove_WithLastPlayer_FirstPlayerIsGivenTurn()
        {
            var firstPlayedTiles = new[] { new DominoTileEntity(11, 12), new DominoTileEntity(1, 2) };
            var secondPlayedTiles = new[] { new DominoTileEntity(10, 12), new DominoTileEntity(3, 2) };
            var sut = CreateGame(firstPlayedTiles, secondPlayedTiles);

            sut.MakeMove(sut.Players.First().Id, firstPlayedTiles.First().Id, sut.Players.First().Train.Id);
            sut.MakeMove(sut.Players.Last().Id, secondPlayedTiles.First().Id, sut.Players.Last().Train.Id);

            Assert.Equal("HasTurnPlayerState", sut.Players.First().GetStateType().Name);
            Assert.Equal("WaitingForTurnPlayerState", sut.Players.Last().GetStateType().Name);
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

            Assert.Equal("HasTurnPlayerState", sut.Players.First().GetStateType().Name);
            Assert.Equal("WaitingForTurnPlayerState", sut.Players.Skip(1).First().GetStateType().Name);
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

            Assert.Equal("WaitingForTurnPlayerState", sut.Players.First().GetStateType().Name);
            Assert.Equal("HasTurnPlayerState", sut.Players.Skip(1).First().GetStateType().Name);
        }

        [Fact]
        public void MakeMove_DoublePlayedAndOherPlayerDoesntPlayOnDouble_IllegalTurn()
        {
            var firstPlayerTiles = new[] { new DominoTileEntity(11, 11), new DominoTileEntity(12, 11), new DominoTileEntity(2, 1) };
            var secondPlayerTiles = new[] { new DominoTileEntity(11, 10), new DominoTileEntity(12, 10) };
            var sut = CreateGame(firstPlayerTiles, secondPlayerTiles);

            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.First().Id,
                sut.MexicanTrain.Id);
            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.Skip(1).First().Id,
                sut.Players.Last().Train.Id);
            Action illegalPlay = () => sut.MakeMove(
                sut.Players.Last().Id,
                secondPlayerTiles.Last().Id,
                sut.Players.Last().Train.Id);

            Assert.ThrowsAny<ApplicationException>(illegalPlay);
        }

        [Fact]
        public void MakeMove_DoublePlayedAndOherPlayerPlaysOnCorrectTrainButWrongTile_IllegalTurn()
        {
            var firstPlayerTiles = new[] { new DominoTileEntity(11, 10), new DominoTileEntity(11, 11), new DominoTileEntity(12, 11), new DominoTileEntity(2, 1) };
            var secondPlayerTiles = new[] { new DominoTileEntity(12, 1), new DominoTileEntity(12, 10) };
            var sut = CreateGame(firstPlayerTiles, secondPlayerTiles);

            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.First().Id,
                sut.MexicanTrain.Id);
            sut.MakeMove(
                sut.Players.Last().Id,
                secondPlayerTiles.First().Id,
                sut.Players.Last().Train.Id);
            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.Skip(1).First().Id,
                sut.MexicanTrain.Id);
            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.Skip(2).First().Id,
                sut.Players.First().Train.Id);
            Action illegalPlay = () => sut.MakeMove(
                sut.Players.Last().Id,
                secondPlayerTiles.Last().Id,
                sut.MexicanTrain.Id);

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

            Assert.Equal("HasTurnPlayerState", sut.Players.Last().GetStateType().Name);
        }

        [Fact]
        public void MakeMove_PlayerDoesntClose_Exception()
        {
            var firstPlayerTiles = new[] { new DominoTileEntity(11, 11), new DominoTileEntity(12, 11), new DominoTileEntity(11, 1), new DominoTileEntity(2, 1) };
            var secondPlayerTiles = new[] { new DominoTileEntity(11, 10), new DominoTileEntity(12, 10) };
            var sut = CreateGame(firstPlayerTiles, secondPlayerTiles);

            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.First().Id,
                sut.MexicanTrain.Id);
            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.Skip(1).First().Id,
                sut.Players.First().Train.Id);
            sut.PassMove(
                sut.Players.Last().Id);
            Action makeMove = () => sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.Skip(2).First().Id,
                sut.Players.First().Train.Id);

            Assert.ThrowsAny<Exception>(makeMove);
        }

        [Fact]
        public void MakeMove_DoubleIsSecondTileOnMexicanTrainMustBeClosed_Exception()
        {
            var firstPlayerTiles = new[] { new DominoTileEntity(11, 11), new DominoTileEntity(12, 11), new DominoTileEntity(12, 1), new DominoTileEntity(12, 2), new DominoTileEntity(6, 7) };
            var secondPlayerTiles = new[] { new DominoTileEntity(12, 10), new DominoTileEntity(12, 9) };
            var sut = CreateGame(firstPlayerTiles, secondPlayerTiles);

            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.Skip(1).First().Id,
                sut.MexicanTrain.Id);
            sut.MakeMove(
                sut.Players.Last().Id,
                secondPlayerTiles.First().Id,
                sut.Players.Last().Train.Id);
            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.First().Id,
                sut.MexicanTrain.Id);
            sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.Skip(2).First().Id,
                sut.Players.Last().Train.Id);
            sut.PassMove(
                sut.Players.Last().Id);
            Action makeMove = () => sut.MakeMove(
                sut.Players.First().Id,
                firstPlayerTiles.Skip(3).First().Id,
                sut.Players.First().Train.Id);

            Assert.ThrowsAny<Exception>(makeMove);
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
            Assert.Equal("OpenPlayerTrainState", ((PlayerTrain)sut.Players.First().Train).GetStateType().Name);
            Assert.Equal("WaitingForTurnPlayerState", sut.Players.First().GetStateType().Name);
            Assert.Equal("HasTurnPlayerState", sut.Players.Skip(1).First().GetStateType().Name);
        }

        [Fact]
        public void PassMoveTwice_WithTwoPlayers_FirstPlayerHasTurn()
        {
            var sut = CreateGame(new DominoTileEntity(11, 12));

            sut.PassMove(sut.Players.First().Id);
            sut.PassMove(sut.Players.Skip(1).First().Id);

            Assert.Equal("WaitingForTurnPlayerState", sut.Players.Skip(1).First().GetStateType().Name);
            Assert.Equal("HasTurnPlayerState", sut.Players.First().GetStateType().Name);
        }

        [Fact]
        public void CreatingGame_TwoPlayers_70TilesInBoneyard()
        {
            var sut = GameEntity.Create(Guid.NewGuid());
            sut.AddPlayer(Guid.NewGuid(), "firstPlayer");
            sut.AddPlayer(Guid.NewGuid(), "secondPlayer");
            var boneyard = sut.boneyard;
            Assert.Equal(
                70,
                boneyard.Count());
        }

        [Fact]
        public void CreatingGame_TwoPlayers_10TilesEach()
        {
            var sut = GameEntity.Create(Guid.NewGuid());
            sut.AddPlayer(Guid.NewGuid(), "firstPlayer");
            sut.AddPlayer(Guid.NewGuid(), "secondPlayer");
            Assert.Equal(
                10,
                sut.Players.First().Hand.Count());
            Assert.Equal(
                10,
                sut.Players.Last().Hand.Count());
        }
        private static GameEntity CreateGame(DominoTileEntity firstPlayedTile)
        {
            return CreateGame(firstPlayedTile, new DominoTileEntity(5, 7));
        }
        private static GameEntity CreateGame(DominoTileEntity firstPlayedTile, DominoTileEntity secondPlayedTile)
        {
            return CreateGame(new[] { firstPlayedTile }, new[] { secondPlayedTile });
        }
        private static GameEntity CreateGame(IEnumerable<DominoTileEntity> firstPlayedTiles, IEnumerable<DominoTileEntity> secondPlayedTiles)
        {
            var gameId = Guid.NewGuid();
            var mexicanTrain = MexicanTrain.Create();
            var engine = new DominoTileEntity(12, 12, true);
            var firstPlayer = PlayerEntity.Create(gameId, Guid.NewGuid(), engine, firstPlayedTiles.ToHashSet(), "playerOne");
            var secondPlayer = PlayerEntity.Create(gameId, Guid.NewGuid(), engine, secondPlayedTiles.ToHashSet(), "playerTwo");
            firstPlayer.GiveTurn();
            var sut = new GameEntity(
                gameId,
                new[] { firstPlayer, secondPlayer },
                mexicanTrain,
                engine,
                new Stack<DominoTileEntity>(new[] { new DominoTileEntity(9, 8), new DominoTileEntity(9, 9) }));
            Games.Add(sut.Id, sut);
            sut.Start();
            return sut;
        }
    }
}
