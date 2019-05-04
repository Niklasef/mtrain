using System;
using Domain.Game;

namespace Server
{
    public class MakeMoveCommand : ICommand
    {
        public MakeMoveCommand(
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
                .MakeMove(
                    PlayerId,
                    TileId,
                    TrainId);
    }
}
