using System;
using System.Collections.Generic;

namespace Domain
{
    public class Player
    {

        public Player(DominoTile engineTile)
        {
            if (engineTile == null)
            {
                throw new ArgumentNullException(nameof(engineTile));
            }

            DominoTiles = new OrderedHashSet<DominoTile>();
            Train = new PlayerTrain(engineTile);
        }

        public Guid Id { get; }
        public ICollection<DominoTile> DominoTiles { get; }
        public PlayerTrain Train { get; }

        internal void AddTile(DominoTile tile)
        {
            DominoTiles.Add(tile);
        }
        internal void RemoveTile(DominoTile tile)
        {
            DominoTiles.Remove(tile);
        }

        public override string ToString()
        {
            return string.Join(",", DominoTiles);
        }
    }
}