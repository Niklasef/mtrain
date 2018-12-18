using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.DominoTile
{
    internal class FullyLinkedState : ITileState
    {
        public IEnumerable<ushort> GetUnlinkedValues(DominoTileEntity tile)
        {
            return Enumerable.Empty<ushort>();
        }

        public void Link(DominoTileEntity linkedTile, DominoTileEntity unlinkedTile)
        {
            throw new ApplicationException($"Can't link tile to fully linked tile");
        }
    }
}