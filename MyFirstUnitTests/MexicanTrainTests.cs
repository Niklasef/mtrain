using Xunit;
using Domain;
using Domain.Train;
using Domain.DominoTile;
using System.Linq;
using System;

namespace MyFirstUnitTests
{
    public class MexicanTrainTests
    {
        [Fact]
        public void AddTile_FirstTileInTrain_CorrectTileExists()
        {
            var sut = MexicanTrain.Create();
            var tile = new DominoTileEntity(1, 2);

            sut.AddTile(tile, Guid.NewGuid());

            Assert.Equal(1, sut.GetTiles().Count());
        }

        [Fact]
        public void AddTile_ThreeHeadMatchingTiles_CorrectTilesExists()
        {
            var sut = MexicanTrain.Create();
            var tile1 = new DominoTileEntity(1, 2);
            var tile2 = new DominoTileEntity(2, 3);
            var tile3 = new DominoTileEntity(3, 4);

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
            var sut = MexicanTrain.Create();
            var tile1 = new DominoTileEntity(1, 2);
            var tile2 = new DominoTileEntity(2, 3);
            var tile3 = new DominoTileEntity(1, 5);

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
            var sut = MexicanTrain.Create();
            var tile1 = new DominoTileEntity(1, 2);
            var tile2 = new DominoTileEntity(4, 5);

            sut.AddTile(tile1, Guid.NewGuid());
            Action action = () => sut.AddTile(tile2, Guid.NewGuid());

            Assert.Throws<ApplicationException>(action);
        }

        [Fact]
        public void AddTile_OpeningAndClosingDouble_CorrectTilesOnTrain()
        {
            var sut = MexicanTrain.Create();
            var tile1 = new DominoTileEntity(11, 0);
            var tile2 = new DominoTileEntity(0, 0);
            var tile3 = new DominoTileEntity(0, 4);

            sut.AddTile(tile1, Guid.NewGuid());
            sut.AddTile(tile2, Guid.NewGuid());
            sut.AddTile(tile3, Guid.NewGuid());
        }

        [Fact]
        public void ToString_MatchesOnRightButUnaligned_AlignedAsMatchingTiles()
        {
            var sut = MexicanTrain.Create();
            var tile1 = new DominoTileEntity(2, 1);
            var tile2 = new DominoTileEntity(3, 1);

            sut.AddTile(tile1, Guid.NewGuid());
            sut.AddTile(tile2, Guid.NewGuid());

            Assert.Equal("[2|1], [1|3]", sut.ToString());
        }

        [Fact]
        public void ToString_MatchesOnLeftButUnaligned_AlignedAsMatchingTiles()
        {
            var sut = MexicanTrain.Create();
            var tile1 = new DominoTileEntity(2, 1);
            var tile2 = new DominoTileEntity(2, 10);

            sut.AddTile(tile1, Guid.NewGuid());
            sut.AddTile(tile2, Guid.NewGuid());

            Assert.Equal("[10|2], [2|1]", sut.ToString());
        }

        [Fact]
        public void ToString_ThreeMatchingNonAlignedTiles_AlignedAsMatchingTiles()
        {
            var sut = MexicanTrain.Create();
            var tile1 = new DominoTileEntity(2, 1);
            var tile2 = new DominoTileEntity(3, 2);
            var tile3 = new DominoTileEntity(3, 4);

            sut.AddTile(tile1, Guid.NewGuid());
            sut.AddTile(tile2, Guid.NewGuid());
            sut.AddTile(tile3, Guid.NewGuid());

            Assert.Equal("[4|3], [3|2], [2|1]", sut.ToString());
        }

        [Fact]
        public void ToString_ThreeMatchingNonAlignedTilesFromBothSides_AlignedAsMatchingTiles()
        {
            var sut = MexicanTrain.Create();
            var tile1 = new DominoTileEntity(1, 2);
            var tile2 = new DominoTileEntity(2, 3);
            var tile3 = new DominoTileEntity(1, 11);

            sut.AddTile(tile1, Guid.NewGuid());
            sut.AddTile(tile2, Guid.NewGuid());
            sut.AddTile(tile3, Guid.NewGuid());

            Assert.Equal("[11|1], [1|2], [2|3]", sut.ToString());
        }
    }
}
