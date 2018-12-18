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
            var engine = new DominoTileEntity(12, 12);
            engine.State = new EngineState();
            var tile = new DominoTileEntity(12, 11);

            tile.Link(engine);

            Assert.Equal(engine, tile.LinkedTiles[0]);
            Assert.Equal(typeof(HalfLinkedState), tile.State.GetType());
            Assert.Equal(1, tile.GetUnlinkedValues().Count());
            Assert.Equal(11, tile.GetUnlinkedValues().First());
        }

        [Fact]
        public void Link_TwoTiles_LinkedRefsAreCorrect()
        {
            var engine = new DominoTileEntity(12, 12);
            engine.State = new EngineState();
            var tileOne = new DominoTileEntity(12, 11);
            var tileTwo = new DominoTileEntity(11, 10);

            tileOne.Link(engine);
            tileTwo.Link(tileOne);

            Assert.True(tileOne.IsLinked(engine));
            Assert.True(tileTwo.IsLinked(tileOne));
            Assert.Equal(typeof(FullyLinkedState), tileOne.State.GetType());
            Assert.Equal(typeof(HalfLinkedState), tileTwo.State.GetType());
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
        public void Link_WhenHalfLinkedToUnorderedTile_NoException()
        {
            var tileOne = new DominoTileEntity(7, 7);
            var tileTwo = new DominoTileEntity(7, 2);
            var tileThree = new DominoTileEntity(12, 2);

            tileOne.Link(tileTwo);
            tileTwo.Link(tileThree);
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
