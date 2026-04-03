using ModelLibrary.Abstract.Types;

namespace ToTheEndOfTheWorld.Gameplay.Events
{
    public readonly record struct ConsumeableUsedEvent(AConsumeable Consumeable);
}
