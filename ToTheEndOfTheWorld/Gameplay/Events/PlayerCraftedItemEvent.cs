using ModelLibrary.Abstract.Types;

namespace ToTheEndOfTheWorld.Gameplay.Events
{
    public readonly record struct PlayerCraftedItemEvent(AType Item, int Count);
}
