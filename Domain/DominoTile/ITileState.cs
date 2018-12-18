using System.Collections.Generic;

namespace Domain.DominoTile
{
    internal interface ITileState
    {
        void Link(DominoTileEntity linkedTile, DominoTileEntity unlinkedTile);
        IEnumerable<ushort> GetUnlinkedValues(DominoTileEntity tile);
    }
}