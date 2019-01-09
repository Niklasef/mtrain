using System;
using Domain.Game;

namespace Server
{
    public class GetGameBoardQuery : IQuery
    {
        public GetGameBoardQuery(
            Guid gameId,
            Guid playerId)
        {
            GameId = gameId;
            PlayerId = playerId;
        }

        public Guid GameId { get; }
        public Guid PlayerId { get; }

        public object Execute() =>
            Games
                .Get(GameId)
                .GetBoard(PlayerId);
    }
}
