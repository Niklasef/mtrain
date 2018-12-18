using System;
using System.Collections.Generic;
using Domain.DominoTile;

namespace Domain.Train
{
    public interface ITrain
    {
        Guid Id { get; }
        void AddTile(DominoTileEntity tile, Guid playerId);
        IEnumerable<DominoTileEntity> GetTiles();
        void ForceAddTile(DominoTileEntity tile);
        Type GetStateType();
    }
}