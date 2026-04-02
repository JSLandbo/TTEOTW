using ModelLibrary.Concrete.Blocks;

namespace ToTheEndOfTheWorld.Context
{
    public readonly record struct WorldTile(long X, long Y);

    public readonly record struct WorldTileBounds(long X, long Y, int Width, int Height);

    public sealed class MiningInteraction(WorldTileBounds tileBounds, Block block = null)
    {
        public WorldTileBounds TileBounds { get; } = tileBounds;
        public Block Block { get; } = block;
    }
}
