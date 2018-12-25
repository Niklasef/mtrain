using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Game
{
    public partial class GameEntity
    {
        private class OpeningDoubleGameState : GameStateBase
        {
            internal override void MakeMove(
                GameEntity game,
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
                if (game.GetPlayedTile(game.GetOpenDoubleTileIds().First()).GetLinkedTiles().Contains(game.GetPlayedTile(tileId)) )
                {
                    game.openDoubles.RemoveAt(0);
                }
                game.state = game.openDoubles.Any()
                    ? game.state = new OpenedDoubleGameState()
                    : new NoDoublesGameState();

                PassTurn(game, playerId);
            }

            internal override void PassMove(GameEntity game, Guid playerId)
            {
                var tile = game
                    .Boneyard
                    .First();
                game.Boneyard
                    .Remove(tile);
                game.GetPlayer(playerId)
                    .PassMove(tile);
                game.state = game.state = new OpenedDoubleGameState();
                PassTurn(game, playerId);
            }
        }
    }
}