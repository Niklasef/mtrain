using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Game;

namespace Server
{
    public class GetGameBoardsQuery : IQuery
    {
        public object Execute() =>
            Games
                .GetAll()
                .Select(g => 
                    g.GetBoard())
                .ToArray();
    }
}
