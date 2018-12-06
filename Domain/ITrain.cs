using System;
using System.Collections.Generic;

namespace Domain
{
    public interface ITrain
    {
        Guid Id { get; }
        void AddTile(DominoTile tile, Guid playerId);
        IEnumerable<DominoTile> GetTiles();
        void ForceAddTile(DominoTile tile);
    }
}