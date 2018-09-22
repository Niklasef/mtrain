using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class PlayerTrain : ITrain
    {
        public Guid Id { get; }
        private DominoTile head;
        private IPlayerTrainState state;

        public PlayerTrain(DominoTile engineTile)
        {
            if (engineTile.State.GetType() != typeof(EngineState))
            {
                throw new ArgumentException($"A player train has to start with the Engine tile. Given tile had state: '{engineTile.State}'", nameof(engineTile));
            }
            Id = Guid.NewGuid();
            head = engineTile ?? throw new ArgumentNullException(nameof(engineTile));
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

        public void AddTile(DominoTile tile)
        {
            state.AddTile(tile, this);
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

        private class OpenPlayerTrainState : IPlayerTrainState
        {
            public void AddTile(DominoTile tile, PlayerTrain playerTrain)
            {
                tile.Link(playerTrain.head);
                playerTrain.head = tile;
            }
        }

        private class ClosedPlayerTrainState : IPlayerTrainState
        {
            public void AddTile(DominoTile tile, PlayerTrain playerTrain)
            {
                throw new ApplicationException("Can't add tile to a closed player train.");
            }
        }

        private interface IPlayerTrainState
        {
            void AddTile(DominoTile tile, PlayerTrain playerTrain);
        }
    }
}