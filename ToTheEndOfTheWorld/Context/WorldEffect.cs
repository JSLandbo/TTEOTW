namespace ToTheEndOfTheWorld.Context
{
    public enum WorldEffectType
    {
        Explosion
    }

    public sealed class WorldEffect(WorldEffectType type, WorldTile tile, int playbackFrames)
    {
        public WorldEffectType Type { get; } = type;
        public WorldTile Tile { get; } = tile;
        public int PlaybackFramesRemaining { get; private set; } = playbackFrames;
        public int TotalPlaybackFrames { get; } = playbackFrames;
        public int PlayedFrames => TotalPlaybackFrames - PlaybackFramesRemaining;

        public bool AdvanceFrame()
        {
            PlaybackFramesRemaining--;
            return PlaybackFramesRemaining <= 0;
        }
    }
}
