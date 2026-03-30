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

    public sealed class WorldInteraction
    {
        public WorldInteraction(
            WorldInteractionType type,
            WorldTileBounds tileBounds,
            Block block = null,
            bool isDestructible = false)
        {
            Type = type;
            TileBounds = tileBounds;
            Block = block;
            IsDestructible = isDestructible;
        }

        public WorldInteractionType Type { get; }
        public WorldTileBounds TileBounds { get; }
        public Block Block { get; }
        public bool IsDestructible { get; }
    }
}
