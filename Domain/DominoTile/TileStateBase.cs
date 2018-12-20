using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Domain;

namespace Domain.DominoTile
{
    public partial class DominoTileEntity
    {
        private abstract class TileStateBase
        {
            internal virtual void Link(DominoTileEntity linkedTile, DominoTileEntity unlinkedTile)
            {
                throw new IllegalStateActionException(GetType());
            }
        }
    }
}