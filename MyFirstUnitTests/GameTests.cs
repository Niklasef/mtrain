using Xunit;
using Domain;
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
            var playedTile = new DominoTile(11, 12);
            var sut = CreateGame(playedTile);

            sut.MakeMove(sut.Players.First().Id, playedTile.Id, sut.Players.First().Train.Id);

            Assert.Equal(playedTile, sut.Players.First().Train.GetTiles().First());
            Assert.Equal(typeof(Player.WaitingForTurnState), sut.Players.First().GetStateType());
            Assert.Equal(typeof(Player.HasTurnState), sut.Players.Skip(1).First().GetStateType());
        }

        [Fact]
        public void MakeMove_WithLastPlayer_FirstPlayerIsGivenTurn()
        {
            var firstPlayedTile = new DominoTile(11, 12);
            var secondPlayedTile = new DominoTile(10, 12);
            var sut = CreateGame(firstPlayedTile, secondPlayedTile);

            sut.MakeMove(sut.Players.First().Id, firstPlayedTile.Id, sut.Players.First().Train.Id);
            sut.MakeMove(sut.Players.Last().Id, secondPlayedTile.Id, sut.Players.Last().Train.Id);

            Assert.Equal(typeof(Player.HasTurnState), sut.Players.First().GetStateType());
            Assert.Equal(typeof(Player.WaitingForTurnState), sut.Players.Last().GetStateType());
        }

        [Fact]
        public void MakeMove_WithDoubleOnMexTrain_TurnIsNotPassed()
        {
            var playedTile = new DominoTile(11, 11);
            var sut = CreateGame(playedTile);

            sut.MakeMove(
                sut.Players.First().Id,
                playedTile.Id,
                sut.MexicanTrain.Id);

            Assert.Equal(typeof(Player.HasTurnState), sut.Players.First().GetStateType());
            Assert.Equal(typeof(Player.WaitingForTurnState), sut.Players.Skip(1).First().GetStateType());
        }

        [Fact]
        public void MakeMove_WithDoubleOnMexTrainAndSecondFreeMoveIsMade_TurnIsPassed()
        {
            var playedTiles = new[] { new DominoTile(11, 11), new DominoTile(12, 11) };
            var sut = CreateGame(playedTiles, new[] { new DominoTile(2, 1) });

            sut.MakeMove(
                sut.Players.First().Id,
                playedTiles.First().Id,
                sut.MexicanTrain.Id);
            sut.MakeMove(
                sut.Players.First().Id,
                playedTiles.Last().Id,
                sut.Players.First().Train.Id);

            Assert.Equal(typeof(Player.WaitingForTurnState), sut.Players.First().GetStateType());
            Assert.Equal(typeof(Player.HasTurnState), sut.Players.Skip(1).First().GetStateType());
        }

        [Fact]
        public void MakeMove_DoublePlayedAndOherPlayerDoesntPlayOnDouble_IllegalTurn()
        {
            var firstPlayerTiles = new[] { new DominoTile(11, 11), new DominoTile(12, 11) };
            var secondPlayerTiles = new[] { new DominoTile(11, 10), new DominoTile(12, 10) };
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
        public void MakeMove_DoublePlayedAndOherPlayerPlayOnDouble_GameStateIsNoDoubles()
        {
            var firstPlayerTiles = new[] { new DominoTile(11, 11), new DominoTile(12, 11) };
            var secondPlayerTiles = new[] { new DominoTile(11, 10), new DominoTile(12, 10) };
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
                secondPlayerTiles.Last().Id,
                sut.MexicanTrain.Id);

            Assert.Equal("OpenedDoubleGameState", sut.GetStateType().Name);
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
        public void MakeMove_PlayerWithTurnPutsTileOnClosedPlayerTrain_Exception()
        {
            var playedTile = new DominoTile(11, 12);
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
            var playedTile = new DominoTile(11, 12);
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
            var sut = CreateGame(new DominoTile(11, 12));

            sut.PassMove(sut.Players.First().Id);

            Assert.Equal(2, sut.Players.First().Hand.Count);
            Assert.Equal(typeof(PlayerTrain.OpenPlayerTrainState), ((PlayerTrain)sut.Players.First().Train).state.GetType());
            Assert.Equal(typeof(Player.WaitingForTurnState), sut.Players.First().GetStateType());
            Assert.Equal(typeof(Player.HasTurnState), sut.Players.Skip(1).First().GetStateType());
        }

        [Fact]
        public void PassMoveTwice_WithTwoPlayers_FirstPlayerHasTurn()
        {
            var sut = CreateGame(new DominoTile(11, 12));

            sut.PassMove(sut.Players.First().Id);
            sut.PassMove(sut.Players.Skip(1).First().Id);

            Assert.Equal(typeof(Player.HasTurnState), sut.Players.First().GetStateType());
            Assert.Equal(typeof(Player.WaitingForTurnState), sut.Players.Skip(1).First().GetStateType());
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
        private static MexicanTrainGame CreateGame(DominoTile firstPlayedTile)
        {
            return CreateGame(firstPlayedTile, new DominoTile(5, 7));
        }
        private static MexicanTrainGame CreateGame(DominoTile firstPlayedTile, DominoTile secondPlayedTile)
        {
            return CreateGame(new[] { firstPlayedTile }, new[] { secondPlayedTile });
        }
        private static MexicanTrainGame CreateGame(IEnumerable<DominoTile> firstPlayedTiles, IEnumerable<DominoTile> secondPlayedTiles)
        {
            var gameId = Guid.NewGuid();
            var mexicanTrain = new MexicanTrain();
            var firstPlayer = new Player(gameId, "playerOne", firstPlayedTiles.ToHashSet());
            var secondPlayer = new Player(gameId, "playerTwo", secondPlayedTiles.ToHashSet());
            firstPlayer.GiveTurn();
            var engine = new DominoTile(12, 12);
            engine.State = new EngineState();
            var sut = new MexicanTrainGame(
                gameId,
                new[] { firstPlayer, secondPlayer },
                mexicanTrain,
                engine,
                new[] { new DominoTile(9, 8), new DominoTile(9, 9) }.ToList());
            Games.Add(sut.Id, sut);
            return sut;
        }
    }
}
