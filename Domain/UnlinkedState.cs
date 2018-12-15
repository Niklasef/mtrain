using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    internal class UnlinkedState : ITileState
    {

        public IEnumerable<ushort> GetUnlinkedValues(DominoTile tile)
        {
            return tile.GetValues();
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
            if(!otherTile.IsLinked(tile)){
                otherTile.Link(tile);
            }
            tile.State = new HalfLinkedState();
        }
    }
}