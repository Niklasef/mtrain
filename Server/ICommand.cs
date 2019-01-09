using System;
using Domain.Game;

namespace Server
{
    public interface ICommand
    {
        void Execute();
    }

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

    public class StartGameCommand : ICommand
    {
        public StartGameCommand(Guid gameId)
        {
            GameId = gameId;
        }

        public Guid GameId { get; }

        public void Execute()
        {
            Games
                .Get(GameId)
                .Start();
        }
    }
}
