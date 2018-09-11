using System;

namespace Domain
{
    public class DominoTile{
        public ushort FirstValue{get;private set;}
        public ushort SecondValue{get;private set;}
        public DominoTile(ushort firstValue, ushort secondValue)
        {
            if(firstValue > 12){
                throw new ArgumentException($"Value must be 0-12, was '{firstValue}'", nameof(firstValue));
            }
            if(secondValue > 12){
                throw new ArgumentException($"Value must be 0-12, was '{secondValue}'", nameof(secondValue));
            }
            FirstValue = firstValue;
            SecondValue = secondValue;
        }

        public override bool Equals(object obj)
        {
            var domineTile = obj as DominoTile;
            if (domineTile == null)
            {
                return false;
            }
            if(domineTile.FirstValue == FirstValue && domineTile.SecondValue == SecondValue){
                return true;
            }
            return domineTile.FirstValue == SecondValue && domineTile.SecondValue == FirstValue;
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + FirstValue.GetHashCode();
                hash = hash * 23 + SecondValue.GetHashCode();
                return hash;
            }
        }

        public override string ToString(){
            return $"First value: {FirstValue}, Second value: {SecondValue}";
        }

        internal void Flip()
        {
            var tempValue = FirstValue;
            FirstValue = SecondValue;
            SecondValue = tempValue;
        }
    }
}