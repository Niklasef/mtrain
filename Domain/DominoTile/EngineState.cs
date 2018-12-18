using System;
using System.Collections.Generic;

namespace Domain.DominoTile
{
    internal class EngineState : ITileState
    {
        public IEnumerable<ushort> GetUnlinkedValues(DominoTileEntity tile)
        {
            return new[] { tile.FirstValue };
        }

        public void Link(DominoTileEntity linkedTile, DominoTileEntity unlinkedTile) { }
    }
}