using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ToTheEndOfTheWorld.Gameplay.Audio
{
    public sealed class ConcurrentOneShotSoundEffectPlayer(GameAudioService audioService, int defaultMaxConcurrent, IReadOnlyDictionary<SoundEffectId, int> maxConcurrentOverrides = null)
    {
        private const double ExpirationPaddingSeconds = 1.0 / 60.0;
        private readonly IReadOnlyDictionary<SoundEffectId, int> maxConcurrentOverrides = maxConcurrentOverrides ?? new Dictionary<SoundEffectId, int>();
        private readonly Dictionary<SoundEffectId, Queue<long>> activeUntilById = [];

        public void Play(SoundEffectId id, float volume = 1.0f)
        {
            long now = Stopwatch.GetTimestamp();

            if (!activeUntilById.TryGetValue(id, out Queue<long> activeUntil))
            {
                activeUntil = new Queue<long>();

                activeUntilById[id] = activeUntil;
            }

            while (activeUntil.Count > 0 && activeUntil.Peek() <= now)
            {
                activeUntil.Dequeue();
            }

            int maxConcurrent = maxConcurrentOverrides.TryGetValue(id, out int overrideValue) ? overrideValue : defaultMaxConcurrent;

            if (activeUntil.Count >= maxConcurrent) return;

            double durationSeconds = audioService.GetDurationSeconds(id);

            if (durationSeconds <= 0.0) return;

            audioService.PlayOneShot(id, volume);

            long expiresAt = now + (long)Math.Ceiling((durationSeconds + ExpirationPaddingSeconds) * Stopwatch.Frequency);

            activeUntil.Enqueue(expiresAt);
        }

        public void Clear()
        {
            activeUntilById.Clear();
        }
    }
}