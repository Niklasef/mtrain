using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    internal class HalfLinkedState : ITileState
    {
        public IEnumerable<ushort> GetUnlinkedValue(DominoTile tile)
        {
            return new[] {
                tile
                .GetValues()
                .First(x => !tile
                    .LinkedTiles
                    .First(t => t != null)
                    .GetValues()
                    .Any(y => x == y))};
        }

        public void Link(DominoTile tile, DominoTile otherTile)
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
            otherTile.AddLinkedTile(tile);
            tile.State = new FullyLinkedState();
        }
    }

    internal class FullyLinkedState : ITileState
    {
        public IEnumerable<ushort> GetUnlinkedValue(DominoTile tile)
        {
            return Enumerable.Empty<ushort>();
        }

        public void Link(DominoTile linkedTile, DominoTile unlinkedTile)
        {
            throw new ApplicationException($"Can't link tile to fully linked tile");
        }
    }
}