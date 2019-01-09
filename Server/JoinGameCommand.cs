using System;
using Domain.Game;

namespace Server
{
    public class JoinGameCommand : ICommand
    {
        public JoinGameCommand(Guid gameId, Guid playerId, string playerName)
        {
            GameId = gameId;
            PlayerId = playerId;
            PlayerName = playerName;
        }

        public Guid PlayerId { get; }
        public string PlayerName { get; }
        public Guid GameId { get; }

        public void Execute() =>
            Games
                .Get(GameId)
                .AddPlayer(PlayerId, PlayerName);
    }
}
