using System;
using Domain.Game;

namespace Server
{
    public class DrawTileCommand : ICommand
    {
        public DrawTileCommand(
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
                .DrawTile(PlayerId);
    }
}
