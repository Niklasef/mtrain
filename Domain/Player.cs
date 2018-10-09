using System;
using System.Collections.Generic;

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
        internal void MakeMove(DominoTile tile, Guid trainId)
        {
            throw new ApplicationException($"Is in illigal state: {GetType().Name}");
        }
        internal virtual bool CanDoMove()
        {
            throw new ApplicationException($"Is in illigal state: {GetType().Name}");
        }

        private class WaitingForTurnState : PlayerState
        {
            internal override bool HasTurn() => false;
            internal override void GiveTurn(Player player)
            {
                player.state = new HasTurnState();
            }
            internal override bool CanDoMove() => false;
        }

        private class HasTurnState : PlayerState
        {
            internal override bool HasTurn() => true;
            internal override void GiveTurn(Player player) { }
            internal override void MakeMove(Player player, DominoTile tile, Guid trainId)
            {
                if (!player.CanDoMove())
                {
                    throw new ApplicationException($"Player '{player.Name}' can´t do a legal move.");
                }
                throw new NotImplementedException();
                player.state = new WaitingForTurnState();
            }
            internal override bool CanDoMove()
            {
                throw new NotImplementedException();
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
            internal virtual void MakeMove(Player player, DominoTile tile, Guid trainId)
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
            internal virtual bool CanDoMove()
            {
                throw new ApplicationException($"Is in illigal state: {GetType().Name}");
            }
        }
    }
}