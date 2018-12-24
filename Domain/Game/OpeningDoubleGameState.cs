using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Game
{
    public partial class MexicanTrainGame
    {
        private class OpeningDoubleGameState : GameStateBase
        {
            internal override void MakeMove(
                MexicanTrainGame game,
                Guid playerId,
                long tileId,
                Guid trainId)
            {
                var train = game.GetTrain(trainId);
                game.GetPlayer(playerId)
                    .ForceMove(tileId, train);//TODO: Possible to move this to shared security assembly?
                if (game.GetPlayedTile(tileId).IsDouble())
                {
                    game.openDoubles.Add(new Tuple<Guid, Guid, long>(trainId, playerId, tileId));
                    return;
                }
                game.state = game.openDoubles.Any()
                    ? game.state = new OpenedDoubleGameState()
                    : new NoDoublesGameState();

                PassTurn(game, playerId);
            }

            internal override void PassMove(MexicanTrainGame game, Guid playerId)
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