using System;
using System.Linq;

namespace Domain.Game
{
    public partial class GameEntity
    {
        private class NoDoublesGameState : GameStateBase
        {
            internal override void MakeMove(GameEntity game, Guid playerId, long tileId, Guid trainId)
            {
                var train = game.GetTrain(trainId);
                game.GetPlayer(playerId)
                    .MakeMove(tileId, train);
                if (game.GetPlayedTile(tileId).IsDouble())
                {
                    game.openDoubles
                        .Add(
                            new Tuple<Guid, Guid, long>(
                                trainId, 
                                playerId, 
                                tileId));
                    game.state = new OpeningDoubleGameState();
                    return;
                }
                if (game.GetPlayer(playerId).Hand.Count == 0)
                {
                    game.state = new WonGameState(game.Id, playerId);
                }
                PassTurn(game, playerId);
            }

            override internal void DrawTile(GameEntity game, Guid playerId)
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