using System;
using System.Collections.Generic;

namespace Domain
{
    internal class EngineState : ITileState
    {
        public IEnumerable<ushort> GetUnlinkedValues(DominoTile tile)
        {
            return new[] { tile.FirstValue };
        }

        public void Link(DominoTile linkedTile, DominoTile unlinkedTile) { }
    }
}