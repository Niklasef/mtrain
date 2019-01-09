using System.Collections.Generic;
using System.Linq;
using Domain.Game;

namespace Server
{
    public class GetGameBoardsQuery : IQuery<IEnumerable<GameBoard>>
    {
        public IEnumerable<GameBoard> Execute() =>
            Games
                .GetAll()
                .Select(g => 
                    g.GetBoard());
    }
}
