using System;
using Domain.Game;

namespace Server
{
    public class CreateGameCommand : ICommand
    {
        public CreateGameCommand(Guid gameId)
        {
            GameId = gameId;
        }

        public Guid GameId { get; }

        public void Execute() =>
            Games
                .Add(
                    GameId,
                    GameEntity.Create(GameId));
    }
}
