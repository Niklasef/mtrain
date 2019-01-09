using System;
using Domain.Game;

namespace Server
{
    public class GetGameBoardAsObserverQuery : IQuery
    {
        public GetGameBoardAsObserverQuery(Guid gameId)
        {
            GameId = gameId;
        }

        public Guid GameId { get; }

        public object Execute() =>
            Games
                .Get(GameId)
                .GetBoard();
    }
}
