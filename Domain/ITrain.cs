using System.Collections.Generic;

namespace Domain
{
    public interface ITrain
    {
        void AddTile(DominoTile tile);
        IEnumerable<DominoTile> GetTiles();
    }
}