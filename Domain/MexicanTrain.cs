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
                System.Console.WriteLine($"start. head: {head}, tail: {tail}");
                return;
            }
            if (head.MatchesUnlinkedValue(tile))
            {
                tile.Link(head);
                head = tile;
                System.Console.WriteLine($"head. head: {head}, tail: {tail}");
                return;
            }
            if (tail.MatchesUnlinkedValue(tile))
            {
                tail.Link(tile);
                tail = tile;
                System.Console.WriteLine($"tail. head: {head}, tail: {tail}");
                return;
            }
        }

        public IEnumerable<DominoTile> GetTiles()
        {
            return GetTiles(head, new List<DominoTile>());
        }

        private IEnumerable<DominoTile> GetTiles(DominoTile tile, List<DominoTile> tiles)
        {
            tiles.Add(tile);
            if(tile != tail)
            {
                return GetTiles(tile.LinkedTiles.First(t => !tiles.Contains(t)), tiles);
            }
            return tiles;
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
    }
}