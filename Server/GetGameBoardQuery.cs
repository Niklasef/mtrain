using System;
using Domain.Game;

namespace Server
{
    public class GetGameBoardQuery : IQuery<GameBoard>
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

        public GameBoard Execute() =>
            Games
                .Get(GameId)
                .GetBoard(PlayerId);
    }
}
