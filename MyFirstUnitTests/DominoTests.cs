using System;
using Xunit;
using Domain;
using Domain.DominoTile;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace MyFirstUnitTests
{
    public class DominoTests
    {
        [Fact]
        public void IsMatch_NoMatchingValue_False()
        {
            var tileOne = new DominoTileEntity(1, 10);
            var tileTwo = new DominoTileEntity(2, 9);

            var result = tileOne.IsMatch(tileTwo);

            Assert.False(result);
        }

        [Fact]
        public void IsMatch_FirstValueMatches_True()
        {
            var tileOne = new DominoTileEntity(1, 10);
            var tileTwo = new DominoTileEntity(1, 9);

            var result = tileOne.IsMatch(tileTwo);

            Assert.True(result);
        }

        [Fact]
        public void IsMatch_TwoValuesMatches_True()
        {
            var tileOne = new DominoTileEntity(1, 10);
            var tileTwo = new DominoTileEntity(1, 1);

            var result = tileOne.IsMatch(tileTwo);

            Assert.True(result);
        }

        [Fact]
        public void Flipping_IsSameTile()
        {
            var tile = new DominoTileEntity(1, 10);
            var tileTwo = new DominoTileEntity(1, 10);
            tileTwo.Flip();

            Assert.Equal(tile, tileTwo);
        }

        [Fact]
        public void Flipping_IdRemainsSame()
        {
            var tile = new DominoTileEntity(1, 10);
            var tileTwo = new DominoTileEntity(1, 10);
            tileTwo.Flip();

            Assert.Equal(tile.Id, tileTwo.Id);
        }

        [Fact]
        public void Link_EngineWithMatching_CorrectLink()
        {
            var engine = new DominoTileEntity(12, 12, true);
            var tile = new DominoTileEntity(12, 11);

            tile.Link(engine);

            Assert.Equal(engine, tile.GetLinkedTiles().First());
            Assert.Equal("HalfLinkedState", tile.GetStateType().Name);
            Assert.Equal(1, tile.GetUnlinkedValues().Count());
            Assert.Equal(11, tile.GetUnlinkedValues().First());
        }

        [Fact]
        public void Link_TwoMatching_CorrectLink()
        {
            var tileOne = new DominoTileEntity(1, 2);
            var tileTwo = new DominoTileEntity(2, 3);

            tileTwo.Link(tileOne);

            Assert.Equal(tileOne, tileTwo.GetLinkedTiles().First());
            Assert.Equal("HalfLinkedState", tileTwo.GetStateType().Name);
            Assert.Equal(1, tileTwo.GetUnlinkedValues().Count());
            Assert.Equal(3, tileTwo.GetUnlinkedValues().First());

            Assert.Equal(tileTwo, tileOne.GetLinkedTiles().First());
            Assert.Equal("HalfLinkedState", tileOne.GetStateType().Name);
            Assert.Equal(1, tileOne.GetUnlinkedValues().Count());
            Assert.Equal(1, tileOne.GetUnlinkedValues().First());            
        }

        [Fact]
        public void GetUnlinkedValues_InUnlinkedState_CorrectUnlinkedValue()
        {
            var tileOne = new DominoTileEntity(2, 1);

            Assert.Equal(2, tileOne.GetUnlinkedValues().Count());
            Assert.True(tileOne.GetUnlinkedValues().Any(v => v == 1));
            Assert.True(tileOne.GetUnlinkedValues().Any(v => v == 2));
        }

        [Fact]
        public void Link_ThreeTiles_LinkedRefsAreCorrect()
        {
            var engine = new DominoTileEntity(12, 12, true);
            var tileOne = new DominoTileEntity(12, 11);
            var tileTwo = new DominoTileEntity(11, 10);

            tileOne.Link(engine);
            tileTwo.Link(tileOne);

            Assert.True(tileOne.IsLinked(engine));
            Assert.True(tileTwo.IsLinked(tileOne));
            Assert.Equal("FullyLinkedState", tileOne.GetStateType().Name);
            Assert.Equal("HalfLinkedState", tileTwo.GetStateType().Name);
            Assert.Equal(1, tileTwo.GetUnlinkedValues().Count());
            Assert.Equal(10, tileTwo.GetUnlinkedValues().First());
        }

        [Fact]
        public void Link_TilesOnEachSide_NoUnlinkedValues()
        {
            var tileOne = new DominoTileEntity(1, 2);
            var tileTwo = new DominoTileEntity(2, 3);
            var tileThree = new DominoTileEntity(1, 4);

            tileOne.Link(tileTwo);
            tileOne.Link(tileThree);

            Assert.Equal(0, tileOne.GetUnlinkedValues().Count());
        }

        [Fact]
        public void Link_OnAlreadyLinkedValue_ThrowsException()
        {
            var tileOne = new DominoTileEntity(1, 2);
            var tileTwo = new DominoTileEntity(2, 3);
            var tileThree = new DominoTileEntity(2, 4);

            tileOne.Link(tileTwo);
            Action linkThreeOnTwo = () => tileTwo.Link(tileThree);
            Action linkTwoOnThree = () => tileThree.Link(tileTwo);

            Assert.Equal(1, tileTwo.GetUnlinkedValues().Count());
            Assert.Equal(3, tileTwo.GetUnlinkedValues().First());
            Assert.Equal("HalfLinkedState", tileTwo.GetStateType().Name);
            Assert.ThrowsAny<Exception>(linkThreeOnTwo);
            Assert.ThrowsAny<Exception>(linkTwoOnThree);
        }

        [Fact]
        public void Link_WhenHalfLinkedToUnorderedTile_TilesAreFlippedSoValuesAlign()
        {
            var engine = new DominoTileEntity(7, 7, isEngine: true);
            var tileTwo = new DominoTileEntity(2, 7);
            var tileThree = new DominoTileEntity(9, 2);

            tileTwo.Link(engine);
            tileThree.Link(tileTwo);

            Assert.Equal(7, tileTwo.FirstValue);
            Assert.Equal(2, tileThree.FirstValue);
        }

        [Fact]
        public void Link_DoubleWithTileOnEachSide_CorrectSequence()
        {
            var tile1 = new DominoTileEntity(11, 0);
            var tile2 = new DominoTileEntity(0, 0);
            var tile3 = new DominoTileEntity(0, 4);

            tile1.Link(tile2);
            tile2.Link(tile3);
        }
    }
}
