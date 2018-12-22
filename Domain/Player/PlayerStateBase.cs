using Domain.DominoTile;
using Domain.Train;

namespace Domain.Player
{
    public partial class PlayerEntity
    {
        private abstract class PlayerStateBase
        {
            internal virtual void GiveTurn(PlayerEntity player) =>
                throw new IllegalStateActionException(GetType());

            internal virtual void EndTurn(PlayerEntity player) =>
                throw new IllegalStateActionException(GetType());

            internal virtual void MakeMove(PlayerEntity player, long tileId, ITrain train) =>
                throw new IllegalStateActionException(GetType());

            internal virtual void ForceMove(PlayerEntity player, long tileId, ITrain train) =>
                throw new IllegalStateActionException(GetType());

            internal virtual void PassMove(PlayerEntity player, DominoTileEntity tile) =>
                throw new IllegalStateActionException(GetType());
        }
    }
}