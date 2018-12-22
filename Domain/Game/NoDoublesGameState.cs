using System;
using System.Linq;

namespace Domain.Game
{
    public partial class MexicanTrainGame
    {
        private class NoDoublesGameState : GameStateBase
        {
            internal override void MakeMove(MexicanTrainGame game, Guid playerId, long tileId, Guid trainId)
            {
                var train = game.GetTrain(trainId);
                game.GetPlayer(playerId)
                    .MakeMove(tileId, train);
                if (game.GetPlayedTile(tileId).IsDouble())
                {
                    game.state = new OpeningDoubleGameState(new Tuple<Guid, Guid>(trainId, playerId));
                    return;
                }
                if (game.GetPlayer(playerId).Hand.Count == 0)
                {
                    game.state = new WonGameState(game.Id, playerId);
                }
                PassTurn(game, playerId);
            }

            override internal void PassMove(MexicanTrainGame game, Guid playerId)
            {
                var tile = game
                    .Boneyard
                    .First();
                game.Boneyard
                    .Remove(tile);
                game.GetPlayer(playerId)
                    .PassMove(tile);
                PassTurn(game, playerId);
            }
        }
    }
}