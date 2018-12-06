using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class PlayerTrain : ITrain
    {
        public Guid Id { get; }
        private readonly Guid ownerId;
        private readonly Guid gameId;
        private DominoTile head;
        protected internal PlayerTrainStateBase state;

        public PlayerTrain(Guid gameId, Guid ownerId)
        {
            var engineTile = Games.Get(gameId).Engine;
            if (engineTile.State.GetType() != typeof(EngineState))
            {
                throw new ArgumentException($"A player train has to start with the Engine tile. Given tile had state: '{engineTile.State}'", nameof(engineTile));
            }
            Id = Guid.NewGuid();
            head = engineTile ?? throw new ArgumentNullException(nameof(engineTile));
            this.ownerId = ownerId;
            state = new ClosedPlayerTrainState(gameId);
        }

        internal void Open()
        {
            state = new OpenPlayerTrainState();
        }

        public void AddTile(DominoTile tile, Guid playerId)
        {
            state.AddTile(tile, this, playerId);
        }

        public void ForceAddTile(DominoTile tile)
        {
            state.ForceAddTile(tile, this);
        }

        public override string ToString()
        {
            return string.Join(", ", GetTiles()
                .Select(t =>
                {
                    if (t.State.GetType() != typeof(EngineState) && t.FirstValue != t.LinkedTiles[0].SecondValue)//TODO: fix hardcoded choise
                    {
                        t.Flip();
                    }
                    return t;
                }
                )
                .Reverse());
        }

        public IEnumerable<DominoTile> GetTiles()
        {
            return GetTiles(new List<DominoTile>(), head);
        }

        private IEnumerable<DominoTile> GetTiles(List<DominoTile> list, DominoTile tile)
        {
            list.Add(tile);
            if (tile.State.GetType() == typeof(EngineState))
            {
                return list;
            }
            return GetTiles(list, tile.LinkedTiles[0]);//TODO: fix hardcoded choise
        }

        protected internal class OpenPlayerTrainState : PlayerTrainStateBase { }

        protected internal class ClosedPlayerTrainState : PlayerTrainStateBase
        {
            private readonly Guid gameId;

            public ClosedPlayerTrainState(Guid gameId)
            {
                this.gameId = gameId;
            }

            public override void AddTile(DominoTile tile, PlayerTrain playerTrain, Guid playerId)
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
            public virtual void AddTile(DominoTile tile, PlayerTrain playerTrain, Guid playerId)
            {
                tile.Link(playerTrain.head);
                playerTrain.head = tile;
            }

            internal void ForceAddTile(DominoTile tile, PlayerTrain playerTrain)
            {
                tile.Link(playerTrain.head);
                playerTrain.head = tile;
            }
        }
    }
}