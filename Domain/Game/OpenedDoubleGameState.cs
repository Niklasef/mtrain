using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Game
{
    public partial class MexicanTrainGame
    {
        private class OpenedDoubleGameState : GameStateBase
        {
            private readonly List<Tuple<Guid, Guid>> openTrains;

            public OpenedDoubleGameState(List<Tuple<Guid, Guid>> openTrains)
            {
                this.openTrains = openTrains;
            }

            internal override void MakeMove(MexicanTrainGame game, Guid playerId, long tileId, Guid trainId)
            {
                if (openTrains.First().Item2 != playerId && openTrains.First().Item1 != trainId)
                {
                    throw new ApplicationException("Illegal move, must play on first open double.");
                }
                var train = game.GetTrain(trainId);
                game.GetPlayer(playerId)
                    .MakeMove(tileId, train);
                if (game.GetPlayedTile(tileId).IsDouble())
                {
                    openTrains.Add(new Tuple<Guid, Guid>(trainId, playerId));
                }
                else
                {
                    openTrains.RemoveAt(0);
                }
                if (!openTrains.Any())
                {
                    game.state = new NoDoublesGameState();
                }
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