using System;
using System.Collections.Generic;
using Domain.DominoTile;
using Domain.Train;

namespace Domain.Game
{
    public class GameBoard
    {
        internal GameBoard(
            Dictionary<Guid, string> players,
            Type gameState,
            Guid gameId,
            IEnumerable<ITrain> trains, 
            IEnumerable<DominoTileEntity> hand,
            Guid playerIdWithTurn)
        {
            Players = players ?? throw new ArgumentNullException(nameof(players));
            GameState = gameState ?? throw new ArgumentNullException(nameof(gameState));
            GameId = gameId;
            Trains = trains ?? throw new ArgumentNullException(nameof(trains));
            Hand = hand ?? throw new ArgumentNullException(nameof(hand));
            PlayerIdWithTurn = playerIdWithTurn;
        }

        public Dictionary<Guid, string> Players { get; }
        public IEnumerable<ITrain> Trains { get; }
        public IEnumerable<DominoTileEntity> Hand { get; }
        public Guid PlayerIdWithTurn { get; }
        public Type GameState { get; }
        public Guid GameId { get; }
    }
}