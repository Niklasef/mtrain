using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.DominoTile
{
    public partial class DominoTileEntity
    {
        private TileStateBase state;
        public ushort FirstValue { get; private set; }
        public ushort SecondValue { get; private set; }
        public long Id => GetHashCode();
        public IEnumerable<DominoTileEntity> GetLinkedTiles() => linkedTiles;
        protected List<DominoTileEntity> linkedTiles = new List<DominoTileEntity>();

        public DominoTileEntity(ushort firstValue, ushort secondValue, bool isEngine = false)
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
            state = isEngine
                ? (TileStateBase)new EngineState()
                : (TileStateBase)new UnlinkedState();
        }

        internal Type GetStateType() =>
            state.GetType();

        internal bool IsLinked(DominoTileEntity tile) =>
            linkedTiles.Contains(tile);

        internal bool MatchesUnlinkedValue(DominoTileEntity tile) =>
            GetUnlinkedValues()
                .Any(x =>
                    tile.GetUnlinkedValues()
                        .Any(y => x == y));

        public override bool Equals(object other) =>
            other as DominoTileEntity == null
                ? false
                : this.Id == ((DominoTileEntity)other).Id;

        public override int GetHashCode()
        {
            unchecked
            {
                var largestValue = Math.Max(FirstValue, SecondValue);
                var smallestValue = Math.Min(FirstValue, SecondValue);
                int hash = 17;
                hash = hash * 23 + largestValue.GetHashCode();
                hash = hash * 23 + smallestValue.GetHashCode();
                return hash;
            }
        }

        public override string ToString() =>
            $"[{FirstValue}|{SecondValue}]";

        internal void Flip()
        {
            var tempValue = FirstValue;
            FirstValue = SecondValue;
            SecondValue = tempValue;
        }

        internal IEnumerable<ushort> GetValues() =>
            new[] { FirstValue, SecondValue };

        internal IEnumerable<ushort> GetUnlinkedValues() =>
            IsDouble() && linkedTiles.Count == 1
                ? new[] { FirstValue }
                : GetValues()
                .Where(v =>
                    linkedTiles
                        .SelectMany(lt => lt.GetValues())
                        .All(ltv => v != ltv));

        internal virtual void Link(DominoTileEntity tile) =>
            state.Link(this, tile);

        public bool IsMatch(DominoTileEntity otherTile) =>
            GetValues()
                .Any(v =>
                    otherTile
                        .GetValues()
                        .Any(ov => v == ov));

        internal bool IsDouble() =>
            FirstValue == SecondValue;
    }
}