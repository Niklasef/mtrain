using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    internal class LinkedState : ITileState
    {
        public IEnumerable<ushort> GetUnlinkedValue(DominoTile tile)
        {
            return new[]{ tile.GetValues()
                .First(x => !tile.LinkedTile
                    .GetValues()
                    .Any(y => x == y))};
        }

        public void Link(DominoTile linkedTile, DominoTile unlinkedTile)
        {
            throw new ApplicationException($"Can't link already linked tile");
        }
    }
}