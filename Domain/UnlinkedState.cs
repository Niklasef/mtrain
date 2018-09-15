using System;
using System.Collections.Generic;

namespace Domain
{
    internal class UnlinkedState : ITileState
    {
        public DominoTile GetLinkedTile(DominoTile tile)
        {
            throw new ApplicationException("Current tile is unlinked");
        }

        public IEnumerable<ushort> GetUnlinkedValue(DominoTile tile)
        {
            return tile.GetValues();
        }

        public void Link(DominoTile unlinkedTile, DominoTile linkedTile)
        {
            unlinkedTile.Link(linkedTile);
            unlinkedTile.State = new LinkedState();
        }
    }
}