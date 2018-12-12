using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Player
    {
        private PlayerState state = new WaitingForTurnState();
        internal Type GetStateType()
        {
            return state.GetType();
        }
        private readonly Guid gameId;
        public Guid Id { get; private set; }

        protected internal HashSet<DominoTile> Hand { get; private set; }

        internal Player(Guid gameId, string name, HashSet<DominoTile> hand)
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
            return string.Join(",", Hand);
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
        internal void PassMove(DominoTile tile)
        {
            state.PassMove(this, tile);
        }

        protected internal class WaitingForTurnState : PlayerState
        {
            internal override bool HasTurn() => false;
            internal override void GiveTurn(Player player) =>
                player.state = new HasTurnState();
            internal override void EndTurn(Player player) { }
        }

        protected internal class HasTurnState : PlayerState
        {
            internal override bool HasTurn() => true;
            internal override void EndTurn(Player player) =>
                player.state = new WaitingForTurnState();
            internal override void GiveTurn(Player player) { }
            internal override void MakeMove(Player player, long tileId, ITrain train)
            {
                var tile = player
                    .Hand
                    .First(t => t.Id == tileId);

                train.AddTile(
                    tile,
                    player.Id);
                player
                    .Hand
                    .Remove(tile);
            }
            internal override void ForceMove(Player player, long tileId, ITrain train)
            {
                var tile = player
                    .Hand
                    .First(t => t.Id == tileId);

                train.ForceAddTile(tile);
                player
                    .Hand
                    .Remove(tile);
            }
            internal override void PassMove(Player player, DominoTile tile)
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
            internal virtual void GiveTurn(Player player)
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
            internal virtual void EndTurn(Player player)
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
            internal virtual void MakeMove(Player player, long tileId, ITrain train)
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
            internal virtual void ForceMove(Player player, long tileId, ITrain train)
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
            internal virtual void PassMove(Player player, DominoTile tile)
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
        }

    }
}