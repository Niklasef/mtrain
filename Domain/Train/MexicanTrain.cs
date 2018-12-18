using System;
using System.Collections.Generic;
using System.Linq;
using Domain.DominoTile;

namespace Domain.Train
{
    public class MexicanTrain : ITrain
    {
        public Guid Id { get; }
        private DominoTileEntity head;
        private DominoTileEntity tail;

        internal MexicanTrain()
        {
            Id = Guid.NewGuid();
        }

        public void AddTile(DominoTileEntity tile, Guid playerId)
        {
            ForceAddTile(tile);
        }

        public void ForceAddTile(DominoTileEntity tile)
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

        public IEnumerable<DominoTileEntity> GetTiles()
        {
            return head != null
                ? GetTiles(head, new List<DominoTileEntity>())
                : Enumerable.Empty<DominoTileEntity>();
        }

        private IEnumerable<DominoTileEntity> GetTiles(DominoTileEntity tile, List<DominoTileEntity> tiles)
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

        public Type GetStateType()
        {
            throw new NotImplementedException();
        }
    }
}