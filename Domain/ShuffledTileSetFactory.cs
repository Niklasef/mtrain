using System;
using System.Linq;
using System.Collections.Generic;
using Domain.DominoTile;

namespace Domain
{

    internal class ShuffledTileSetFactory
    {
        internal ShuffledTileSetFactory()
        {
        }

        internal ICollection<DominoTileEntity> Create(short engineValue)
        {
            var tiles = new OrderedHashSet<DominoTileEntity>();
            ushort secondValueStart = 0;
            for (ushort firstIndex = 0; firstIndex <= 12; firstIndex++)
            {
                for (ushort secondIndex = secondValueStart; secondIndex <= 12; secondIndex++)
                {
                    tiles.Add(new DominoTileEntity(firstIndex, secondIndex, firstIndex == secondIndex && firstIndex == engineValue));
                }
                secondValueStart++;
            }

            Random randomizer = new Random();
            var randomizedTiles = new DominoTileEntity[tiles.Count()];
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
            return new OrderedHashSet<DominoTileEntity>(randomizedTiles);
        }
    }
}