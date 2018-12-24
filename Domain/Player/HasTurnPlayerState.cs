using System;
using System.Collections.Generic;
using System.Linq;
using Domain.DominoTile;
using Domain.Game;
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

                var openDoubleIds = Games
                    .Get(player.gameId)
                    .GetOpenDoubleTileIds()
                    .ToArray();
                if(openDoubleIds.Any() && 
                    !Games.Get(player.gameId).GetPlayedTile(openDoubleIds.First()).IsMatch(tile))
                {
                    throw new ApplicationException($"Illegal move. Must play on first present open double which is '{Games.Get(player.gameId).GetPlayedTile(openDoubleIds.First())}'. The played tile '{tile}' is not a match.");
                }

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