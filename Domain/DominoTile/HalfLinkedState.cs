using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.DominoTile
{
    public partial class DominoTileEntity
    {
        private class HalfLinkedState : TileStateBase
        {
            internal override void Link(DominoTileEntity tile, DominoTileEntity otherTile)
            {
                if (tile == null)
                {
                    throw new ArgumentNullException(nameof(tile));
                }
                if (otherTile == null)
                {
                    throw new ArgumentNullException(nameof(otherTile));
                }
                if (tile.IsLinked(otherTile))
                {
                    throw new ApplicationException($"Can't link: '{tile}' with '{otherTile}' because tiles are already linked");
                }
                if (!tile.IsMatch(otherTile))
                {
                    throw new ApplicationException($"Illegal move, no matching values. Can't link: '{tile}' with '{otherTile}'");
                }
                if (!tile.HasMatchingUnlinkedValue(otherTile) && !otherTile.IsLinked(tile))
                {
                    throw new ApplicationException($"Illegal move, no matching unlinked values. Can't link: '{tile}' with '{otherTile}'");
                }
                tile.linkedTiles.Add(otherTile);
                tile.state = new FullyLinkedState();
                if (!otherTile.IsLinked(tile))
                {
                    otherTile.Link(tile);
                }
            }
        }
    }
}