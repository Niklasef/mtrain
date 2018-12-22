using System;
using Domain.DominoTile;

namespace Domain.Train
{
    public partial class PlayerTrain
    {
        private class OpenPlayerTrainState : PlayerTrainStateBase
        {
            internal override void AddTile(
                PlayerTrain playerTrain, 
                DominoTileEntity tile, 
                Guid playerId)
            {
                base.AddTile(
                    playerTrain, 
                    tile);
                if (playerId == playerTrain.ownerId)
                {
                    playerTrain.state = new ClosedPlayerTrainState(); ;
                }
            }

            internal override void ForceAddTile(
                PlayerTrain playerTrain,
                DominoTileEntity tile
            ) => AddTile(playerTrain, tile);
        }
    }
}