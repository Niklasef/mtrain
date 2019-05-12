using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Game
{
    public partial class GameEntity
    {
        private class OpenedDoubleGameState : GameStateBase
        {
            internal override void MakeMove(GameEntity game, Guid playerId, long tileId, Guid trainId)
            {
                if (game.openDoubles.First().Item1 != trainId)
                {
                    throw new ApplicationException("Illegal move, must play on first open double.");
                }
                var train = game.GetTrain(trainId);
                game.GetPlayer(playerId)
                    .MakeMove(tileId, train);
                if (game.GetPlayedTile(tileId).IsDouble())
                {
                    game.openDoubles.Add(new Tuple<Guid, Guid, long>(trainId, playerId, tileId));
                }
                else
                {
                    game.openDoubles.RemoveAt(0);
                }
                if (!game.openDoubles.Any())
                {
                    game.state = new NoDoublesGameState();
                }
                PassTurn(game, playerId);
            }

            internal override void DrawTile(GameEntity game, Guid playerId)
            {
                var tile = game
                    .boneyard
                    .Pop();
                game.GetPlayer(playerId)
                    .PassMove(tile);
                PassTurn(game, playerId);
            }
        }
    }
}