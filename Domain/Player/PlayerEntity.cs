using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.DominoTile;
using Domain.Train;

namespace Domain.Player
{
    public class PlayerEntity
    {
        private PlayerState state = new WaitingForTurnState();
        internal Type GetStateType()
        {
            return state.GetType();
        }
        private readonly Guid gameId;
        public Guid Id { get; private set; }

        protected internal HashSet<DominoTileEntity> Hand { get; private set; }

        internal PlayerEntity(Guid gameId, string name, HashSet<DominoTileEntity> hand)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Must be a real name of a player.", nameof(name));
            }
            Id = Guid.NewGuid();
            this.Hand = hand ?? throw new ArgumentNullException(nameof(hand));
            this.gameId = gameId;
            Name = name;
        }

        public string Name { get; }
        private PlayerTrain train;
        public ITrain Train
        {
            get
            {
                return InnerGetTrain();
            }
        }

        private PlayerTrain InnerGetTrain()
        {
            if (train == null)
            {
                train = new PlayerTrain(gameId, Id);
            }
            return train;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"PlayerEntity: {Name}");
            stringBuilder.AppendLine($"State: {GetStateType().Name}");
            stringBuilder.AppendLine($"Hand:");
            stringBuilder.AppendLine(
                string.Join(",", Hand));
            stringBuilder.AppendLine($"Train:");
            stringBuilder.AppendLine(
                string.Join(",", Train));
            return stringBuilder.ToString();
        }

        public bool HasTurn()
        {
            return state.HasTurn();
        }
        internal void GiveTurn()
        {
            state.GiveTurn(this);
        }
        internal void EndTurn()
        {
            state.EndTurn(this);
        }
        internal void MakeMove(long tileId, ITrain train)
        {
            state.MakeMove(this, tileId, train);
        }
        internal void ForceMove(long tileId, ITrain train)
        {
            state.ForceMove(this, tileId, train);
        }
        internal void PassMove(DominoTileEntity tile)
        {
            state.PassMove(this, tile);
        }

        protected internal class WaitingForTurnState : PlayerState
        {
            internal override bool HasTurn() => false;
            internal override void GiveTurn(PlayerEntity player) =>
                player.state = new HasTurnState();
            internal override void EndTurn(PlayerEntity player) { }
        }

        protected internal class HasTurnState : PlayerState
        {
            internal override bool HasTurn() => true;
            internal override void EndTurn(PlayerEntity player) =>
                player.state = new WaitingForTurnState();
            internal override void GiveTurn(PlayerEntity player) { }
            internal override void MakeMove(PlayerEntity player, long tileId, ITrain train)
            {
                var tile = player
                    .Hand
                    .First(t => t.Id == tileId);

                train.AddTile(
                    tile,
                    player.Id);
                player.Hand = 
                    new HashSet<DominoTileEntity>(
                        player.Hand.Where(t => t.Id != tile.Id));
            }
            internal override void ForceMove(PlayerEntity player, long tileId, ITrain train)
            {
                var tile = player
                    .Hand
                    .First(t => t.Id == tileId);

                train.ForceAddTile(tile);
                player.Hand = 
                    new HashSet<DominoTileEntity>(
                        player.Hand.Where(t => t.Id != tile.Id));
            }
            internal override void PassMove(PlayerEntity player, DominoTileEntity tile)
            {
                player.Hand.Add(tile);
                player.InnerGetTrain().Open();
            }
        }

        protected internal abstract class PlayerState
        {
            internal virtual bool HasTurn()
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
            internal virtual void GiveTurn(PlayerEntity player)
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
            internal virtual void EndTurn(PlayerEntity player)
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
            internal virtual void MakeMove(PlayerEntity player, long tileId, ITrain train)
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
            internal virtual void ForceMove(PlayerEntity player, long tileId, ITrain train)
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
            internal virtual void PassMove(PlayerEntity player, DominoTileEntity tile)
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
        }

    }
}