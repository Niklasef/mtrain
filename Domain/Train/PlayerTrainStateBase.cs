using System;
using Domain.DominoTile;

namespace Domain.Train
{
    public partial class PlayerTrain
    {
        private abstract class PlayerTrainStateBase
        {
            internal virtual void AddTile(
                PlayerTrain playerTrain, 
                DominoTileEntity tile, 
                Guid playerId)
            {
                throw new IllegalStateActionException(GetType());
            }

            internal virtual void ForceAddTile(
                PlayerTrain playerTrain,
                DominoTileEntity tile)
            {
                throw new IllegalStateActionException(GetType());
            }

            protected void AddTile(
                PlayerTrain playerTrain,
                DominoTileEntity tile)
            {
                tile.Link(playerTrain.head);
                playerTrain.head = tile;
            }
        }
    }
}