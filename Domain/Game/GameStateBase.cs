using System;
using System.Linq;

namespace Domain.Game
{
    public partial class MexicanTrainGame
    {
        private abstract class GameStateBase
        {
            internal virtual void MakeMove(MexicanTrainGame mexicanTrainGame, Guid playerId, long tileId, Guid trainId) =>
                throw new IllegalStateActionException(GetType());

            internal virtual void PassMove(MexicanTrainGame mexicanTrainGame, Guid playerId) =>
                throw new IllegalStateActionException(GetType());

            protected void PassTurn(MexicanTrainGame game, Guid currentPlayerId)
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