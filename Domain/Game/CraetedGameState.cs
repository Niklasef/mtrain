using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Player;

namespace Domain.Game
{
    public partial class GameEntity
    {
        private class CraetedGameState : GameStateBase
        {
            internal override void AddPlayer(
                GameEntity game,
                Guid playerId,
                string playerName
            ) =>
                game
                    .players
                    .Add(
                        PlayerEntity.Create(
                            game.Id,
                            playerId,
                            game.Engine,
                            new HashSet<DominoTile.DominoTileEntity>(
                                game.boneyard.PopRange(10)),
                            playerName));

            internal override void Start(GameEntity game)
            {
                game.Players
                    .First()
                    .GiveTurn();
                game.state = new NoDoublesGameState();
            }
        }
    }
}