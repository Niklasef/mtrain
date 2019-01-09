using System.Collections.Generic;

namespace Domain
{
    public static class StackExtensions
    {
        public static IEnumerable<T> PopRange<T>(this Stack<T> stack, uint count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return stack.Pop();
            }
        }
    }
}