using System;
using Domain.Game;

namespace Server
{
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
