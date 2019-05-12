using System;
using Domain.DominoTile;

namespace Domain.Train
{

    public partial class PlayerTrain
    {
        private class ClosedPlayerTrainState : PlayerTrainStateBase
        {
            internal override void AddTile(
                PlayerTrain playerTrain,
                DominoTileEntity tile,
                Guid playerId)
            {
                if (playerId != playerTrain.ownerId)
                {
                    throw new ApplicationException("Can't add tile to a closed player train.");
                }
                base.AddTile(
                    playerTrain, 
                    tile);
                if (tile.IsDouble())
                {
                    playerTrain.state = new OpenPlayerTrainState();
                }
            }

            internal override void ForceAddTile(
                PlayerTrain playerTrain,
                DominoTileEntity tile
            ) => base.AddTile(playerTrain, tile);
        }
    }
}