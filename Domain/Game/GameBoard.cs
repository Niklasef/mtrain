using System;
using System.Collections.Generic;
using Domain.DominoTile;
using Domain.Train;

namespace Domain.Game
{
    public class GameBoard
    {
        internal GameBoard(
            IEnumerable<ITrain> trains, 
            IEnumerable<DominoTileEntity> hand,
            Guid playerIdWithTurn)
        {
            Trains = trains;
            Hand = hand;
            PlayerIdWithTurn = playerIdWithTurn;
        }

        public IEnumerable<ITrain> Trains { get; }
        public IEnumerable<DominoTileEntity> Hand { get; }
        public Guid PlayerIdWithTurn { get; }
    }
}