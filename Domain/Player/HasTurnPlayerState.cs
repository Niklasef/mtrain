using System.Collections.Generic;
using System.Linq;
using Domain.DominoTile;
using Domain.Train;

namespace Domain.Player
{
    public partial class PlayerEntity
    {
        private class HasTurnPlayerState : PlayerStateBase
        {
            internal override void EndTurn(PlayerEntity player) =>
                player.state = new WaitingForTurnPlayerState();
                
            internal override void GiveTurn(PlayerEntity player) { }
            internal override void MakeMove(PlayerEntity player, long tileId, ITrain train)
            {
                var tile = player
                    .Hand
                    .First(t => t.Id == tileId);

                train.AddTile(
                    tile,
                    player.Id);
                player.Hand =
                    new HashSet<DominoTileEntity>(
                        player.Hand.Where(t => t.Id != tile.Id));
            }
            internal override void ForceMove(PlayerEntity player, long tileId, ITrain train)
            {
                var tile = player
                    .Hand
                    .First(t => t.Id == tileId);

                train.ForceAddTile(tile);
                player.Hand =
                    new HashSet<DominoTileEntity>(
                        player.Hand.Where(t => t.Id != tile.Id));
            }
            internal override void PassMove(PlayerEntity player, DominoTileEntity tile)
            {
                player.Hand.Add(tile);
                player.InnerGetTrain().Open();
            }
        }
    }
}