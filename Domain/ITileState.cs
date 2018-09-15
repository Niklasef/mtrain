using System.Collections.Generic;

namespace Domain
{
    internal interface ITileState
    {
        void Link(DominoTile linkedTile, DominoTile unlinkedTile);
        IEnumerable<ushort> GetUnlinkedValue(DominoTile tile);
        DominoTile GetLinkedTile(DominoTile tile);
    }
}