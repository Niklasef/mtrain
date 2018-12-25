using System;

namespace Domain.Game
{

    public partial class GameEntity
    {
        private class WonGameState : GameStateBase
        {
            private readonly Guid gameId;

            public Guid WinnerId { get; }
            public WonGameState(Guid gameId, Guid winnerId)
            {
                this.gameId = gameId;
                WinnerId = winnerId;
            }

            public override string ToString() =>
                $"Winner is: '{Games.Get(gameId).GetPlayer(WinnerId).Name}'";
        }
    }
}