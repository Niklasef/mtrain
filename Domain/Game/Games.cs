using System;
using System.Collections.Generic;
using Domain.DominoTile;
using Domain.Train;
using Domain.Player;
using System.Linq;

namespace Domain.Game
{
    public static class Games
    {
        private static readonly IDictionary<Guid, GameEntity> innerList = new Dictionary<Guid, GameEntity>();

        public static IEnumerable<GameEntity> GetAll() =>
            innerList.Values;

        public static GameEntity Get(Guid key)
        {
            if (!innerList.ContainsKey(key))
            {
                throw new ApplicationException($"Game with id {key} doesn't exist.");
            }
            return innerList[key];
        }

        public static void Add(Guid key, GameEntity value)
        {
            innerList.Add(key, value);
        }

        internal static void Remove(Guid id)
        {
            innerList.Remove(id);
        }
    }
}