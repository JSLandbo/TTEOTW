using Microsoft.Xna.Framework;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public readonly record struct PlayerIntent(Vector2 MovementInput, Vector2 FacingDirection);
}
