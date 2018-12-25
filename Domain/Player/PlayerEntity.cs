using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DominoTile;
using Domain.Train;

namespace Domain.Player
{
    public partial class PlayerEntity
    {
        private PlayerStateBase state = new WaitingForTurnPlayerState();

        public Type GetStateType() =>
            state.GetType();

        public Guid Id { get; private set; }

        protected internal HashSet<DominoTileEntity> Hand { get; private set; }

        internal PlayerEntity(
            DominoTileEntity engineTile,
            Guid gameId,
            string name,
            HashSet<DominoTileEntity> hand)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Must be a real name of a player.", nameof(name));
            }
            Id = Guid.NewGuid();
            this.Hand = hand ?? throw new ArgumentNullException(nameof(hand));
            train = new PlayerTrain(engineTile, Id);
            this.gameId = gameId;
            Name = name;
        }

        public string Name { get; }
        private PlayerTrain train;
        private readonly Guid gameId;

        public ITrain Train =>
            InnerGetTrain();

        private PlayerTrain InnerGetTrain() =>
            train;

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"PlayerEntity: {Name}");
            stringBuilder.AppendLine($"State: {GetStateType().Name}");
            stringBuilder.AppendLine($"Hand:");
            stringBuilder.AppendLine(
                string.Join(", ", Hand.Select((e, i) => $"{i + 1}:{e}")));
            stringBuilder.AppendLine($"Train:");
            stringBuilder.AppendLine(Train.ToString());
            return stringBuilder.ToString();
        }

        public bool HasTurn() =>
            state is HasTurnPlayerState;

        internal void GiveTurn() =>
            state.GiveTurn(this);

        internal void EndTurn() =>
            state.EndTurn(this);

        internal void MakeMove(long tileId, ITrain train) =>
            state.MakeMove(this, tileId, train);

        internal void ForceMove(long tileId, ITrain train) =>
            state.ForceMove(this, tileId, train);

        internal void PassMove(DominoTileEntity tile) =>
            state.PassMove(this, tile);

    }
}