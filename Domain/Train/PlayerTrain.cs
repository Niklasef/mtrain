using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DominoTile;
using Domain.Game;

namespace Domain.Train
{
    public class PlayerTrain : ITrain
    {
        public Guid Id { get; }
        private readonly Guid ownerId;
        private readonly Guid gameId;
        private DominoTileEntity head;
        protected internal PlayerTrainStateBase state;

        public PlayerTrain(Guid gameId, Guid ownerId)
        {
            this.gameId = gameId;
            Id = Guid.NewGuid();
            head = Games.Get(gameId).Engine;
            this.ownerId = ownerId;
            state = new ClosedPlayerTrainState();
        }

        internal void Open()
        {
            state = new OpenPlayerTrainState();
        }

        public void AddTile(DominoTileEntity tile, Guid playerId)
        {
            state.AddTile(tile, this, playerId);
        }

        public void ForceAddTile(DominoTileEntity tile)
        {
            state.ForceAddTile(tile, this);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Train state: {state.GetType().Name}");
            stringBuilder.AppendLine(
                string.Join(", ", GetTiles()
                    .Select(t =>
                    {
                        if (t != Games.Get(gameId).Engine && t.FirstValue != t.GetLinkedTiles().First().SecondValue)//TODO: fix hardcoded choise
                        {
                            t.Flip();
                        }
                        return t;
                    })
                    .Reverse()));
            return stringBuilder.ToString();
        }

        public IEnumerable<DominoTileEntity> GetTiles()
        {
            return GetTiles(new List<DominoTileEntity>(), head);
        }

        private IEnumerable<DominoTileEntity> GetTiles(List<DominoTileEntity> list, DominoTileEntity tile)
        {
            list.Add(tile);
            if (tile == Games.Get(gameId).Engine)
            {
                return list;
            }
            return GetTiles(list, tile.GetLinkedTiles().First());//TODO: fix hardcoded choise
        }

        public Type GetStateType()
        {
            return state.GetType();
        }

        protected internal class OpenPlayerTrainState : PlayerTrainStateBase
        {
            public override void AddTile(DominoTileEntity tile, PlayerTrain playerTrain, Guid playerId)
            {
                base.AddTile(tile, playerTrain, playerId);
                if (playerId == playerTrain.ownerId)
                {
                    playerTrain.state = new ClosedPlayerTrainState();;
                }
            }
        }

        protected internal class ClosedPlayerTrainState : PlayerTrainStateBase
        {
            public override void AddTile(DominoTileEntity tile, PlayerTrain playerTrain, Guid playerId)
            {
                if (playerId != playerTrain.ownerId)
                {
                    throw new ApplicationException("Can't add tile to a closed player train.");
                }
                base.AddTile(tile, playerTrain, playerId);
                if (tile.IsDouble())
                {
                    playerTrain.state = new OpenPlayerTrainState();
                }
            }
        }

        protected internal abstract class PlayerTrainStateBase
        {
            public virtual void AddTile(DominoTileEntity tile, PlayerTrain playerTrain, Guid playerId)
            {
                tile.Link(playerTrain.head);
                playerTrain.head = tile;
            }

            internal void ForceAddTile(DominoTileEntity tile, PlayerTrain playerTrain)
            {
                tile.Link(playerTrain.head);
                playerTrain.head = tile;
            }
        }
    }
}