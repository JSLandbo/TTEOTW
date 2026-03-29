using ModelLibrary.Concrete;
using ToTheEndOfTheWorld.Context.StaticRepositories;

namespace ToTheEndOfTheWorld.Gameplay.Events
{
    public readonly record struct WorldBlockDestroyedEvent(World World, short BlockId, WorldTile Location);
}
