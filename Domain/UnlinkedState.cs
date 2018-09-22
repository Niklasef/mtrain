using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    internal class UnlinkedState : ITileState
    {

        public IEnumerable<ushort> GetUnlinkedValue(DominoTile tile)
        {
            return tile.GetValues();
        }

        public void Link(DominoTile unlinkedTile, DominoTile linkedTile)
        {
            if (linkedTile == null)
            {
                throw new ArgumentNullException(nameof(linkedTile));
            }
            if (!linkedTile.Matches(unlinkedTile))
            {
                throw new ApplicationException($"Illegal move, no matching values. Can't link: unlinkedTile: '{unlinkedTile}' with linkedTile: '{linkedTile}'");
            }
            unlinkedTile.LinkedTile = linkedTile;
            unlinkedTile.State = new LinkedState();
        }
    }
}