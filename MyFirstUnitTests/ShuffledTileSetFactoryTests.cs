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
                    .Create()
                    .Count());
        }

        [Fact]
        public void ShufleTiles_TilesAreShuffled()
        {
            var tileStackOne = new ShuffledTileSetFactory().Create();
            var tileStackTwo = new ShuffledTileSetFactory().Create();
            Assert.False(Enumerable.SequenceEqual(tileStackOne, tileStackTwo));
        }

        [Fact]
        public void ShufleTiles_ContainsNoDuplicates()
        {
            var tileStack = new ShuffledTileSetFactory().Create();
            var duplicates = tileStack.Where(x => tileStack.Count(y => x.Equals(y)) > 1);
            Assert.True(
                duplicates.Count() == 0
            );
        }
    }
}
