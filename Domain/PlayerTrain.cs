using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class PlayerTrain : ITrain
    {
        public Guid Id { get; }
        private Guid ownerId;
        private DominoTile head;
        private PlayerTrainStateBase state;

        public PlayerTrain(DominoTile engineTile, Guid ownerId)
        {
            if (engineTile.State.GetType() != typeof(EngineState))
            {
                throw new ArgumentException($"A player train has to start with the Engine tile. Given tile had state: '{engineTile.State}'", nameof(engineTile));
            }
            Id = Guid.NewGuid();
            head = engineTile ?? throw new ArgumentNullException(nameof(engineTile));
            this.ownerId = ownerId;
            state = new ClosedPlayerTrainState();
        }

        internal void Open()
        {
            state = new OpenPlayerTrainState();
        }

        internal void Close()
        {
            state = new ClosedPlayerTrainState();
        }

        public void AddTile(DominoTile tile, Guid playerId)
        {
            state.AddTile(tile, this, playerId);
        }

        public override string ToString()
        {
            return string.Join(", ", GetTiles()
                .Select(t =>
                {
                    if (t.State.GetType() != typeof(EngineState) && t.FirstValue != t.LinkedTile.SecondValue)
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
            return GetTiles(list, tile.LinkedTile);
        }

        public bool IsMatchingTile(DominoTile tile, Guid playerId)
        {
            throw new NotImplementedException();
        }

        private class OpenPlayerTrainState : PlayerTrainStateBase
        {
        }

        private class ClosedPlayerTrainState : PlayerTrainStateBase
        {
            public override void AddTile(DominoTile tile, PlayerTrain playerTrain, Guid playerId)
            {
                if (playerTrain.ownerId == playerId)
                {
                    base.AddTile(tile, playerTrain, playerId);
                    return;
                }
                throw new ApplicationException("Can't add tile to a closed player train.");
            }
        }

        private abstract class PlayerTrainStateBase
        {
            public virtual void AddTile(DominoTile tile, PlayerTrain playerTrain, Guid playerId)
            {
                tile.Link(playerTrain.head);
                playerTrain.head = tile;
            }
        }
    }
}