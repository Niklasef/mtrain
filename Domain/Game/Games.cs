using System;
using System.Collections.Generic;

namespace Domain.Game
{
    public static class Games
    {
        private static readonly IDictionary<Guid, MexicanTrainGame> innerList = new Dictionary<Guid, MexicanTrainGame>();

        public static MexicanTrainGame Get(Guid key)
        {
            if (!innerList.ContainsKey(key))
            {
                throw new ApplicationException($"Game with id {key} doesn't exist.");
            }
            return innerList[key];
        }

        public static void Add(Guid key, MexicanTrainGame value)
        {
            innerList.Add(key, value);
        }

        internal static void Remove(Guid id)
        {
            innerList.Remove(id);
        }
    }
}