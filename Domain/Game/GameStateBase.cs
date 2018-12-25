using System;
using System.Linq;

namespace Domain.Game
{
    public partial class GameEntity
    {
        private abstract class GameStateBase
        {
            internal virtual void MakeMove(GameEntity mexicanTrainGame, Guid playerId, long tileId, Guid trainId) =>
                throw new IllegalStateActionException(GetType());

            internal virtual void PassMove(GameEntity mexicanTrainGame, Guid playerId) =>
                throw new IllegalStateActionException(GetType());

            protected void PassTurn(GameEntity game, Guid currentPlayerId)
            {
                var nextPlayer = game
                    .Players
                    .SkipWhile(p => p.Id != currentPlayerId)
                    .Skip(1)
                    .FirstOrDefault() ?? game.Players.First();
                game.Players
                    .First(p => p.Id == currentPlayerId)
                    .EndTurn();
                nextPlayer
                    .GiveTurn();
            }
        }
    }
}