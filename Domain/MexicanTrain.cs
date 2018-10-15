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

        internal MexicanTrain()
        {
            Id = Guid.NewGuid();
        }

        public void AddTile(DominoTile tile, Guid playerId)
        {
            if (head == null && tail == null)
            {
                head = tail = tile;
                return;
            }
            if (head.MatchesUnlinkedValue(tile))
            {
                tile.Link(head);
                head = tile;
                return;
            }
            if (tail.MatchesUnlinkedValue(tile))
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
            return GetTiles(list, tile.LinkedTiles[0]);//TODO: fix hardcoded choise
        }

        public override string ToString()
        {
            return string.Join(", ",
                GetTiles()
                .Select(t =>
                {
                    if (t != tail && t.FirstValue != t.LinkedTiles[0].SecondValue)//TODO: fix hardcoded choise
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

        public bool IsMatchingTile(DominoTile tile, Guid playerId)
        {
            throw new NotImplementedException();
        }
    }
}