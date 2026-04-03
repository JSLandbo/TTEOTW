using ModelLibrary.Abstract;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public static class PlayerThrusterUsageService
    {
        public static bool UsesThrustersForMovement(APlayer player, bool isGrounded)
        {
            return player.MovementInput.Y < 0 || (!isGrounded && player.MovementInput.X != 0);
        }
    }
}
