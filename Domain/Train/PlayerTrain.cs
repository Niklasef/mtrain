using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DominoTile;
using Domain.Game;

namespace Domain.Train
{
    public partial class PlayerTrain : ITrain
    {
        public Guid Id { get; }
        private readonly Guid ownerId;
        private DominoTileEntity head;
        private DominoTileEntity engineTile;
        private PlayerTrainStateBase state;

        public PlayerTrain(
            DominoTileEntity engineTile, 
            Guid ownerId)
        {
            Id = Guid.NewGuid();
            head = engineTile;
            this.engineTile = engineTile;
            this.ownerId = ownerId;
            state = new ClosedPlayerTrainState();
        }

        internal void Open() =>
            state = new OpenPlayerTrainState();

        public void AddTile(DominoTileEntity tile, Guid playerId) =>
            state.AddTile(this, tile, playerId);

        public void ForceAddTile(DominoTileEntity tile) =>
            state.ForceAddTile(this, tile);

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Train state: {state.GetType().Name}");
            stringBuilder.AppendLine(
                string.Join(", ", GetTiles()
                    .Select(t =>
                    {
                        if (t != engineTile && t.FirstValue != t.GetLinkedTiles().First().SecondValue)//TODO: fix hardcoded choise
                        {
                            t.Flip();
                        }
                        return t;
                    })
                    .Reverse()));
            return stringBuilder.ToString();
        }

        public IEnumerable<DominoTileEntity> GetTiles() =>
            GetTiles(new List<DominoTileEntity>(), head);

        private IEnumerable<DominoTileEntity> GetTiles(List<DominoTileEntity> list, DominoTileEntity tile)
        {
            list.Add(tile);
            return tile == engineTile
                ? list
                : GetTiles(
                    list, 
                    tile.GetLinkedTiles().First());//TODO: fix hardcoded choise
        }

        public Type GetStateType() =>
            state.GetType();
    }
}