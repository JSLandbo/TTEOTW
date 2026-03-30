namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerFallDamageProtectionService
    {
        private const int ProtectionFramesAfterVerticalMining = 4;
        private int framesRemaining;
        private bool isProtectedThisFrame;

        public bool IsProtectedThisFrame => isProtectedThisFrame;

        public void BeginFrame()
        {
            isProtectedThisFrame = framesRemaining > 0;

            if (framesRemaining > 0)
            {
                framesRemaining--;
            }
        }

        public void RefreshAfterVerticalMining()
        {
            framesRemaining = ProtectionFramesAfterVerticalMining;
        }

        public void Clear()
        {
            framesRemaining = 0;
            isProtectedThisFrame = false;
        }
    }
}
