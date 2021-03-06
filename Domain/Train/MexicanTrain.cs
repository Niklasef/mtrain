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

        internal MexicanTrain(Guid id)
        {
            Id = id;
        }

        internal static MexicanTrain Create()
        {
            return new MexicanTrain(Guid.NewGuid());
        }

        public void AddTile(DominoTileEntity tile, Guid playerId) =>
            ForceAddTile(tile);

        public void ForceAddTile(DominoTileEntity tile)
        {
            if (head == null && tail == null)
            {
                head = tail = tile;
                return;
            }
            if (head == tail && tile.GetUnlinkedValues().Any(x => x == head.FirstValue))
            {
                tile.Link(head);
                tail = tile;
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
            throw new ApplicationException($"Illegal move. Head nor tail matches '{tile}'");
        }

        public IEnumerable<DominoTileEntity> GetTiles() =>
            head != null
                ? GetTiles(head, new List<DominoTileEntity>())
                : Enumerable.Empty<DominoTileEntity>();

        private IEnumerable<DominoTileEntity> GetTiles(DominoTileEntity tile, List<DominoTileEntity> tiles)
        {
            tiles.Add(tile);
            return tile != tail
                ? GetTiles(tile.GetLinkedTiles().First(t => !tiles.Contains(t)), tiles)
                : tiles;
        }

        public override string ToString() =>
            string.Join(", ", GetTiles().Reverse());

        public Type GetStateType()
        {
            throw new NotImplementedException();
        }
    }
}