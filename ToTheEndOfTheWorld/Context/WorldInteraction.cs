using ModelLibrary.Concrete.Blocks;

namespace ToTheEndOfTheWorld.Context
{
    public readonly record struct WorldTile(long X, long Y);

    public readonly record struct WorldTileBounds(long X, long Y, int Width, int Height);

    public enum WorldInteractionType
    {
        Mining,
        ExplosionZone
    }

    public sealed class WorldInteraction(WorldInteractionType type, WorldTileBounds tileBounds, Block block = null, bool isDestructible = false)
    {
        public WorldInteractionType Type { get; } = type;
        public WorldTileBounds TileBounds { get; } = tileBounds;
        public Block Block { get; } = block;
        public bool IsDestructible { get; } = isDestructible;
    }
}
