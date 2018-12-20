using System;
using System.Collections.Generic;

namespace Domain.DominoTile
{
    public partial class DominoTileEntity
    {
        private class EngineState : TileStateBase
        {
            internal override void Link(DominoTileEntity linkedTile, DominoTileEntity unlinkedTile) { }
        }
    }
}