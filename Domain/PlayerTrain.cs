using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class PlayerTrain : ITrain
    {
        public Guid Id { get; }
        internal protected DominoTile head;

        public PlayerTrain(DominoTile engineTile)
        {
            if (engineTile.State.GetType() != typeof(EngineState))
            {
                throw new ArgumentException($"A player train has to start with the Engine tile. Given tile had state: '{engineTile.State}'", nameof(engineTile));
            }
            Id = Guid.NewGuid();
            head = engineTile ?? throw new ArgumentNullException(nameof(engineTile));
        }

        public void AddTile(DominoTile tile)
        {
            tile.Link(head);
            head = tile;
        }

        public override string ToString()
        {
            return string.Join(", ", GetTiles()
                .Select(t =>
                {
                    if (t.State.GetType() != typeof(EngineState) && t.FirstValue != t.LinkedTile.SecondValue)
                    {
                        t.Flip();
                    }
                    return t;
                })
                .Reverse());
        }

        public IEnumerable<DominoTile> GetTiles()
        {
            return GetTiles(new List<DominoTile>(), head);
        }

        private IEnumerable<DominoTile> GetTiles(List<DominoTile> list, DominoTile tile)
        {
            list.Add(tile);
            if (tile.State.GetType() == typeof(EngineState))
            {
                return list;
            }
            return GetTiles(list, tile.LinkedTile);
        }
    }
}