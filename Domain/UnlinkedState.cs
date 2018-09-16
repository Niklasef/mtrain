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
            if (linkedTile.State.GetType() != typeof(LinkedState) && linkedTile.State.GetType() != typeof(EngineState))
            {
                throw new ApplicationException($"Can only link to an already linked tile or to the Engine tile. Tile '{linkedTile} didn´t fulfill either requirement'");
            }
            var isMatch = unlinkedTile.GetUnlinkedValue()
                .Any(x => linkedTile
                    .GetUnlinkedValue()
                    .Any(y => x == y));
            if (!isMatch)
            {
                throw new ApplicationException($"Illegal move, no matching values. Can't link: unlinkedTile: '{unlinkedTile}' with linkedTile: '{linkedTile}'");
            }
            unlinkedTile.LinkedTile = linkedTile;
            unlinkedTile.State = new LinkedState();
        }
    }
}