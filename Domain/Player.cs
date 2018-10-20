using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Player
    {
        private PlayerState state = new WaitingForTurnState();
        private readonly Guid gameId;
        public Guid Id { get; private set; }

        internal HashSet<DominoTile> Hand { get; private set; }

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
        public PlayerTrain Train
        {
            get
            {
                if (train == null)
                {
                    train = new PlayerTrain(Games.Get(gameId).Engine, Id);
                }
                return train;
            }
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
        internal void MakeMove(long tileId, ITrain train)
        {
            state.MakeMove(this, tileId, train);
        }

        private class WaitingForTurnState : PlayerState
        {
            internal override bool HasTurn() => false;
            internal override void GiveTurn(Player player)
            {
                player.state = new HasTurnState();
            }
        }

        private class HasTurnState : PlayerState
        {
            internal override bool HasTurn() => true;
            internal override void GiveTurn(Player player) { }
            internal override void MakeMove(Player player, long tileId, ITrain train)
            {
                train.AddTile(player.Hand.First(t => t.Id == tileId), player.Id);
                player.state = new WaitingForTurnState();
            }
        }

        private abstract class PlayerState
        {
            internal virtual bool HasTurn()
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
            internal virtual void GiveTurn(Player player)
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
            internal virtual void MakeMove(Player player, long tileId, ITrain train)
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
        }
    }
}