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
        public void Flipping_SumRemainsSame(){
            var tile = new DominoTile(1, 10);
            var tileTwo = new DominoTile(1, 10);
            tileTwo.Flip();

            Assert.Equal(tile, tileTwo);
        }
    }
}
