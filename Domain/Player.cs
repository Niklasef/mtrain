using System;
using System.Collections.Generic;

namespace Domain
{
    public class Player{

        public Player()
        {
            dominoTiles = new OrderedHashSet<DominoTile>();
        }

        public Guid Id{get;}
        protected internal readonly ICollection<DominoTile> dominoTiles;
        internal void AddTile(DominoTile tile){
            dominoTiles.Add(tile);
        }
        internal void RemoveTile(Guid tileId){}

        public override string ToString()
        {
            return string.Join(",", dominoTiles);
        }
    }
}