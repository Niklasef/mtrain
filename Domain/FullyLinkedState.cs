using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{

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