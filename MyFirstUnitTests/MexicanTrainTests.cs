using Xunit;
using Domain;
using System.Linq;
using System;

namespace MyFirstUnitTests
{
    public class MexicanTrainTests
    {
        [Fact]
        public void AddTile_FirstTileInTrain_CorrectTileExists()
        {
            var sut = new MexicanTrain();
            var tile = new DominoTile(1, 2);

            sut.AddTile(tile, Guid.NewGuid());

            Assert.Equal(1, sut.GetTiles().Count());
        }

        [Fact]
        public void AddTile_ThreeHeadMatchingTiles_CorrectTilesExists()
        {
            var sut = new MexicanTrain();
            var tile1 = new DominoTile(1, 2);
            var tile2 = new DominoTile(2, 3);
            var tile3 = new DominoTile(3, 4);

            sut.AddTile(tile1, Guid.NewGuid());
            sut.AddTile(tile2, Guid.NewGuid());
            sut.AddTile(tile3, Guid.NewGuid());

            Assert.Equal(3, sut.GetTiles().Count());
            Assert.Equal(tile3, sut.GetTiles().First());
            Assert.Equal(tile2, sut.GetTiles().Skip(1).First());
            Assert.Equal(tile1, sut.GetTiles().Skip(2).First());
        }

        [Fact]
        public void AddTile_TilesMatchingBothHeadAndTail_CorrectTilesExists()
        {
            var sut = new MexicanTrain();
            var tile1 = new DominoTile(1, 2);
            var tile2 = new DominoTile(2, 3);
            var tile3 = new DominoTile(1, 5);

            sut.AddTile(tile1, Guid.NewGuid());
            sut.AddTile(tile2, Guid.NewGuid());
            sut.AddTile(tile3, Guid.NewGuid());

            Assert.Equal(3, sut.GetTiles().Count());
            Assert.Equal(tile2, sut.GetTiles().First());
            Assert.Equal(tile1, sut.GetTiles().Skip(1).First());
            Assert.Equal(tile3, sut.GetTiles().Skip(2).First());
        }

        [Fact]
        public void AddTile_MissmatchingTile_ThrowsException()
        {
            var sut = new MexicanTrain();
            var tile1 = new DominoTile(1, 2);
            var tile2 = new DominoTile(4, 5);

            sut.AddTile(tile1, Guid.NewGuid());
            Action action = () => sut.AddTile(tile2, Guid.NewGuid());

            Assert.Throws<ApplicationException>(action);
        }

        [Fact]
        public void ToString_MatchesOnRightButUnaligned_AlignedAsMatchingTiles()
        {
            var sut = new MexicanTrain();
            var tile1 = new DominoTile(2, 1);
            var tile2 = new DominoTile(3, 1);

            sut.AddTile(tile1, Guid.NewGuid());
            sut.AddTile(tile2, Guid.NewGuid());

            Assert.Equal("[2|1], [1|3]", sut.ToString());
        }

        [Fact]
        public void ToString_MatchesOnLeftButUnaligned_AlignedAsMatchingTiles()
        {
            var sut = new MexicanTrain();
            var tile1 = new DominoTile(2, 1);
            var tile2 = new DominoTile(2, 10);

            sut.AddTile(tile1, Guid.NewGuid());
            sut.AddTile(tile2, Guid.NewGuid());

            Assert.Equal("[10|2], [2|1]", sut.ToString());
        }

        [Fact]
        public void ToString_ThreeMatchingNonAlignedTiles_AlignedAsMatchingTiles()
        {
            var sut = new MexicanTrain();
            var tile1 = new DominoTile(2, 1);
            var tile2 = new DominoTile(3, 2);
            var tile3 = new DominoTile(3, 4);

            sut.AddTile(tile1, Guid.NewGuid());
            sut.AddTile(tile2, Guid.NewGuid());
            sut.AddTile(tile3, Guid.NewGuid());

            Assert.Equal("[4|3], [3|2], [2|1]", sut.ToString());
        }

        [Fact]
        public void ToString_ThreeMatchingNonAlignedTilesFromBothSides_AlignedAsMatchingTiles()
        {
            var sut = new MexicanTrain();
            var tile1 = new DominoTile(1, 2);
            var tile2 = new DominoTile(2, 3);
            var tile3 = new DominoTile(1, 11);

            sut.AddTile(tile1, Guid.NewGuid());
            sut.AddTile(tile2, Guid.NewGuid());
            sut.AddTile(tile3, Guid.NewGuid());

            Assert.Equal("[11|1], [1|2], [2|3]", sut.ToString());
        }
    }
}
