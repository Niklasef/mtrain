using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Domain.DominoTile
{
    public partial class DominoTileEntity
    {
        private abstract class TileStateBase : StateBase
        {
            internal virtual void Link(DominoTileEntity linkedTile, DominoTileEntity unlinkedTile)
            {
                throw new IllegalStateActionException(GetType());
            }
        }
    }

    internal abstract class StateBase
    {
    }

    internal class IllegalStateActionException : ApplicationException
    {
        public IllegalStateActionException(Type type) : base($"Can't do this action when in state: '{type.Name}'")
        {
        }
    }
}