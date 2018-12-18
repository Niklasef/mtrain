using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.DominoTile
{
    internal class HalfLinkedState : ITileState
    {
        public IEnumerable<ushort> GetUnlinkedValues(DominoTileEntity tile) =>
            tile.IsDouble()
                ? new[] { tile.FirstValue }
                : new[] {
                    tile
                    .GetValues()
                    .First(x => !tile
                        .LinkedTiles
                        .First(t => t != null)
                        .GetValues()
                        .Any(y => x == y))};

        //TODO almost same impl as unlinked... refactor
        public void Link(DominoTileEntity tile, DominoTileEntity otherTile)
        {
            if (tile == null)
            {
                throw new ArgumentNullException(nameof(tile));
            }
            if (otherTile == null)
            {
                throw new ArgumentNullException(nameof(otherTile));
            }
            if (!tile.MatchesUnlinkedValue(otherTile) || !otherTile.MatchesUnlinkedValue(tile))
            {
                throw new ApplicationException($"Illegal move, no matching unlinked values. Can't link: tile: '{tile}' with tile: '{otherTile}'");
            }
            tile.AddLinkedTile(otherTile);
            if (!otherTile.IsLinked(tile))
            {
                otherTile.Link(tile);
            }
            tile.State = new FullyLinkedState();
        }
    }
}