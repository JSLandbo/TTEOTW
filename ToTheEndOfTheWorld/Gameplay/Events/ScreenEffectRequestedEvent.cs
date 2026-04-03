using ToTheEndOfTheWorld.Context;

namespace ToTheEndOfTheWorld.Gameplay.Events
{
    public readonly record struct ScreenEffectRequestedEvent(ScreenEffectType Type);
}
