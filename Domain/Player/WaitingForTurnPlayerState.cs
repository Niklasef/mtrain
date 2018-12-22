namespace Domain.Player
{
    public partial class PlayerEntity
    {
        private class WaitingForTurnPlayerState : PlayerStateBase
        {
            internal override void GiveTurn(PlayerEntity player) =>
                player.state = new HasTurnPlayerState();

            internal override void EndTurn(PlayerEntity player) { }
        }
    }
}