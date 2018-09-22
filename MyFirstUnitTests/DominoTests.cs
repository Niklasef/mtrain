using System;
using Xunit;
using Domain;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace MyFirstUnitTests
{
    public class DominoTests
    {
        [Fact]
        public void Flipping_SumRemainsSame()
        {
            var tile = new DominoTile(1, 10);
            var tileTwo = new DominoTile(1, 10);
            tileTwo.Flip();

            Assert.Equal(tile, tileTwo);
        }

        [Fact]
        public void Link_EngineWithMatching_CorrectLink()
        {
            var engine = new DominoTile(12, 12);
            engine.State = new EngineState();
            var tile = new DominoTile(12, 11);

            tile.Link(engine);

            Assert.Equal(engine, tile.LinkedTile);
            Assert.Equal(typeof(LinkedState), tile.State.GetType());
            Assert.Equal(1, tile.GetUnlinkedValues().Count());
            Assert.Equal(11, tile.GetUnlinkedValues().First());
        }

        [Fact]
        public void Link_TwoTiles_LinkedRefsAreCorrect()
        {
            var engine = new DominoTile(12, 12);
            engine.State = new EngineState();
            var tileOne = new DominoTile(12, 11);
            var tileTwo = new DominoTile(11, 10);

            tileOne.Link(engine);
            tileTwo.Link(tileOne);

            Assert.Equal(tileOne.LinkedTile, engine);
            Assert.Equal(tileTwo.LinkedTile, tileOne);
            Assert.Equal(typeof(LinkedState), tileTwo.State.GetType());
            Assert.Equal(1, tileTwo.GetUnlinkedValues().Count());
            Assert.Equal(10, tileTwo.GetUnlinkedValues().First());
        }
    }
}
