namespace ToTheEndOfTheWorld.Gameplay.Events
{
    public readonly record struct WorldBlockDestroyedEvent(ModelWorld World, short BlockId, WorldTile Location, WorldBlockDestroyMethod Method);
}
