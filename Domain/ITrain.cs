using System.Collections.Generic;

namespace Domain
{
    public interface ITrain
    {
        IEnumerable<DominoTile> DominoTiles { get; }
        void AddTile(DominoTile tile);
    }
}