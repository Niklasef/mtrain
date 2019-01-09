using System;
using Domain.Game;

namespace Server
{
    public class GetGameBoardAsObserverQuery : IQuery<GameBoard>
    {
        public GetGameBoardAsObserverQuery(Guid gameId)
        {
            GameId = gameId;
        }

        public Guid GameId { get; }

        public GameBoard Execute() =>
            Games
                .Get(GameId)
                .GetBoard();
    }
}
