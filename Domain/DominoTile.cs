using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class DominoTile
    {
        internal ITileState State { get; set; }
        public ushort FirstValue { get; private set; }
        public ushort SecondValue { get; private set; }
        internal DominoTile[] LinkedTiles { get; set; }

        public DominoTile(ushort firstValue, ushort secondValue)
        {
            if (firstValue > 12)
            {
                throw new ArgumentException($"Value must be 0-12, was '{firstValue}'", nameof(firstValue));
            }
            if (secondValue > 12)
            {
                throw new ArgumentException($"Value must be 0-12, was '{secondValue}'", nameof(secondValue));
            }
            FirstValue = firstValue;
            SecondValue = secondValue;
            LinkedTiles = new DominoTile[2];
            State = new UnlinkedState();
        }

        internal void AddLinkedTile(DominoTile linkedTile)
        {
            var firstFreeIndex = LinkedTiles
                .Select((t, i) => new { Tile = t, Index = i })
                .First(t => t.Tile == null)
                .Index;
                //Todo: check if tile already exists...
            LinkedTiles[firstFreeIndex] = linkedTile;
        }

        internal bool MatchesUnlinkedValue(DominoTile tile)
        {
            return GetUnlinkedValues().Any(x => tile.GetUnlinkedValues().Any(y => x == y));
        }

        public override bool Equals(object obj)
        {
            var domineTile = obj as DominoTile;
            if (domineTile == null)
            {
                return false;
            }
            if (domineTile.FirstValue == FirstValue && domineTile.SecondValue == SecondValue)
            {
                return true;
            }
            return domineTile.FirstValue == SecondValue && domineTile.SecondValue == FirstValue;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + FirstValue.GetHashCode();
                hash = hash * 23 + SecondValue.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"[{FirstValue}|{SecondValue}]";
        }

        public void Flip()
        {
            var tempValue = FirstValue;
            FirstValue = SecondValue;
            SecondValue = tempValue;
        }

        internal IEnumerable<ushort> GetValues()
        {
            return new[] { FirstValue, SecondValue };
        }

        internal IEnumerable<ushort> GetUnlinkedValues()
        {
            return State.GetUnlinkedValue(this);
        }

        internal virtual void Link(DominoTile linkedTile)
        {
            State.Link(this, linkedTile);
        }
    }
}