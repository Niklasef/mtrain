using System;
using Domain.Game;

namespace Server
{
    public class PassMoveCommand : ICommand
    {
        public PassMoveCommand(
            Guid gameId,
            Guid playerId,
            Guid trainId,
            long tileId)
        {
            GameId = gameId;
            PlayerId = playerId;
            TrainId = trainId;
            TileId = tileId;
        }

        public Guid GameId { get; }
        public Guid PlayerId { get; }
        public Guid TrainId { get; }
        public long TileId { get; }

        public void Execute() =>
            Games
                .Get(GameId)
                .PassMove(PlayerId);
    }
}
