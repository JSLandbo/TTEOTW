using Microsoft.Xna.Framework;

namespace ToTheEndOfTheWorld.Context
{
    public enum ScreenEffectType
    {
        Explosion
    }

    public sealed class ScreenEffect(ScreenEffectType type, int playbackFrames, Point size)
    {
        public ScreenEffectType Type { get; } = type;
        public int PlaybackFramesRemaining { get; private set; } = playbackFrames;
        public int TotalPlaybackFrames { get; } = playbackFrames;
        public Point Size { get; } = size;
        public int PlayedFrames => TotalPlaybackFrames - PlaybackFramesRemaining;

        public bool AdvanceFrame()
        {
            PlaybackFramesRemaining--;
            return PlaybackFramesRemaining <= 0;
        }
    }

    public readonly record struct ScreenEffectDefinition(Point Size, int PlaybackFrames);
}
