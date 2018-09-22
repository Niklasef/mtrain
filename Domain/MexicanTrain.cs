using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class MexicanTrain : ITrain
    {
        public Guid Id { get; }
        private DominoTile head;
        private DominoTile tail;

        public MexicanTrain()
        {
            Id = Guid.NewGuid();
        }

        public void AddTile(DominoTile tile)
        {
            if (head == null && tail == null)
            {
                head = tail = tile;
                return;
            }
            if (head.Matches(tile))
            {
                tile.Link(head);
                head = tile;
                return;
            }
            if (tail.Matches(tile))
            {
                tail.Link(tile);
                tail = tile;
                return;
            }
        }

        public IEnumerable<DominoTile> GetTiles()
        {
            return GetTiles(new List<DominoTile>(), head);
        }

        private IEnumerable<DominoTile> GetTiles(List<DominoTile> list, DominoTile tile)
        {
            list.Add(tile);
            if (tile == tail)
            {
                return list;
            }
            return GetTiles(list, tile.LinkedTile);
        }

        public override string ToString()
        {
            return string.Join(", ",
                GetTiles()
                .Select(t =>
                {
                    if (t != tail && t.FirstValue != t.LinkedTile.SecondValue)
                    {
                        t.Flip();
                    }
                    return t;
                })
                .ToArray()
                .Select(t =>
                {
                    if (t == tail && t.SecondValue != GetPenultimate().FirstValue)
                    {
                        t.Flip();
                    }
                    return t;
                })
                .Reverse());
        }

        private DominoTile GetPenultimate()
        {
            return GetTiles()
                .ToArray()
                .Reverse()
                .Skip(1)
                .First();
        }
    }
}