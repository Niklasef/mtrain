using System;
using System.Linq;
using System.Collections.Generic;

namespace Domain
{

    internal class ShuffledTileSetFactory
    {
        internal ShuffledTileSetFactory()
        {
        }

        internal ICollection<DominoTile> Create()
        {
            var tiles = new OrderedHashSet<DominoTile>();
            ushort secondValueStart = 0;
            for (ushort firstIndex = 0; firstIndex <= 12; firstIndex++)
            {
                for (ushort secondIndex = secondValueStart; secondIndex <= 12; secondIndex++)
                {
                    tiles.Add(new DominoTile(firstIndex, secondIndex));
                }
                secondValueStart++;
            }

            Random randomizer = new Random();
            var randomizedTiles = new DominoTile[tiles.Count()];
            tiles.CopyTo(randomizedTiles);
            var n = randomizedTiles.Count();
            while (n > 1)
            {
                n--;
                var k = randomizer.Next(n + 1);
                var value = randomizedTiles[k];
                randomizedTiles[k] = randomizedTiles[n];
                randomizedTiles[n] = value;
            }
            return new OrderedHashSet<DominoTile>(randomizedTiles);
        }
    }
}