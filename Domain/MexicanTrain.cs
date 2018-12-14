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
            ForceAddTile(tile);
        }

        public void ForceAddTile(DominoTile tile)
        {
            if (head == null && tail == null)
            {
                head = tail = tile;
                return;
            }
            if (head == tail && tile.GetUnlinkedValues().Any(x => x == head.FirstValue))
            {
                if (tile.SecondValue != head.FirstValue && tile.SecondValue != head.SecondValue)
                {
                    tile.Flip();
                }
                tile.Link(head);
                tail = tile;
                return;
            }
            if (head == tail && tile.GetUnlinkedValues().Any(x => x == head.SecondValue))
            {
                if (tile.FirstValue != head.FirstValue && tile.FirstValue != head.SecondValue)
                {
                    tile.Flip();
                }
                tile.Link(head);
                head = tile;
                return;
            }
            if (head.MatchesUnlinkedValue(tile))
            {
                if (tile.FirstValue != head.FirstValue && tile.FirstValue != head.SecondValue)
                {
                    tile.Flip();
                }
                tile.Link(head);
                head = tile;
                return;
            }
            if (tail.MatchesUnlinkedValue(tile))
            {
                if(tile.SecondValue != tail.FirstValue && tile.SecondValue != tail.SecondValue)
                {
                    tile.Flip();
                }
                tail.Link(tile);
                tail = tile;
                return;
            }
            throw new ApplicationException($"Illegal move. Head nor tail matches '{tile}'");
        }

        public IEnumerable<DominoTile> GetTiles()
        {
            return head != null
                ? GetTiles(head, new List<DominoTile>())
                : Enumerable.Empty<DominoTile>();
        }

        private IEnumerable<DominoTile> GetTiles(DominoTile tile, List<DominoTile> tiles)
        {
            tiles.Add(tile);
            if (tile != tail)
            {
                return GetTiles(tile.LinkedTiles.First(t => !tiles.Contains(t)), tiles);
            }
            return tiles;
        }

        public override string ToString()
        {
            return string.Join(", ", GetTiles().Reverse());
        }
    }
}