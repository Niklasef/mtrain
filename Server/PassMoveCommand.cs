using System;
using Domain.Game;

namespace Server
{
    public class PassMoveCommand : ICommand
    {
        public PassMoveCommand(
            Guid gameId,
            Guid playerId)
        {
            GameId = gameId;
            PlayerId = playerId;
        }

        public Guid GameId { get; }
        public Guid PlayerId { get; }

        public void Execute() =>
            Games
                .Get(GameId)
                .PassMove(PlayerId);
    }
}
