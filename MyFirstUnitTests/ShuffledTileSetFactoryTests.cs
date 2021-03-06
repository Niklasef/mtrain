using Xunit;
using Domain;
using System.Linq;

namespace MyFirstUnitTests
{
    public class ShuffledTileSetFactoryTests
    {
        [Fact]
        public void ShufleTiles_91Tiles()
        {
            Assert.Equal(
                91,
                new ShuffledTileSetFactory()
                    .Create(12)
                    .Count());
        }

        [Fact]
        public void ShufleTiles_TilesAreShuffled()
        {
            var tileStackOne = new ShuffledTileSetFactory().Create(12);
            var tileStackTwo = new ShuffledTileSetFactory().Create(12);
            Assert.False(Enumerable.SequenceEqual(tileStackOne, tileStackTwo));
        }

        [Fact]
        public void ShufleTiles_ContainsNoDuplicates()
        {
            var tileStack = new ShuffledTileSetFactory().Create(12);
            var duplicates = tileStack.Where(x => tileStack.Count(y => x.Equals(y)) > 1);
            Assert.True(
                duplicates.Count() == 0
            );
        }
    }
}
