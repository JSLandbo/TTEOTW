using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace ToTheEndOfTheWorld.Gameplay.Audio
{
    public sealed class GameAudioService
    {
        private readonly Dictionary<MusicTrack, Song> musicTracks = [];
        private readonly Dictionary<SoundEffectId, SoundEffect> soundEffects = [];
        private readonly Dictionary<AudioLoopChannel, SoundEffectInstance> activeLoops = [];
        private readonly Dictionary<AudioLoopChannel, SoundEffectId> activeLoopIds = [];
        private MusicTrack? currentTrack;

        public void LoadContent(ContentManager content)
        {
            TryLoad(content, "Audio/Music/MainTheme", MusicTrack.MainTheme, musicTracks);

            TryLoad(content, "Audio/Sfx/LoopMining", SoundEffectId.LoopMining, soundEffects);
            TryLoad(content, "Audio/Sfx/LoopEngineActive", SoundEffectId.LoopEngineActive, soundEffects);
            TryLoad(content, "Audio/Sfx/LoopThrusterActive", SoundEffectId.LoopThrusterActive, soundEffects);
            TryLoad(content, "Audio/Sfx/EffectExplosion", SoundEffectId.EffectExplosion, soundEffects);
            TryLoad(content, "Audio/Sfx/EffectMinedBlock", SoundEffectId.EffectMinedBlock, soundEffects);
            TryLoad(content, "Audio/Sfx/EffectBoughtFromStore", SoundEffectId.EffectBoughtFromStore, soundEffects);
            TryLoad(content, "Audio/Sfx/EffectSoldToStore", SoundEffectId.EffectSoldToStore, soundEffects);
            TryLoad(content, "Audio/Sfx/EffectUsedFuelCapsule", SoundEffectId.EffectUsedFuelCapsule, soundEffects);
            TryLoad(content, "Audio/Sfx/EffectUsedTrashBin", SoundEffectId.EffectUsedTrashBin, soundEffects);
            TryLoad(content, "Audio/Sfx/EffectYouDied", SoundEffectId.EffectYouDied, soundEffects);
            TryLoad(content, "Audio/Sfx/EffectCoolantPatch", SoundEffectId.EffectCoolantPatch, soundEffects);
            TryLoad(content, "Audio/Sfx/EffectCraftedItem", SoundEffectId.EffectCraftedItem, soundEffects);
            TryLoad(content, "Audio/Sfx/EffectHittingGround", SoundEffectId.EffectHittingGround, soundEffects);
            TryLoad(content, "Audio/Sfx/EffectHullRepairKit", SoundEffectId.EffectHullRepairKit, soundEffects);
            TryLoad(content, "Audio/Sfx/EffectHullDamage", SoundEffectId.EffectHullDamage, soundEffects);
        }

        public void PlayMusic(MusicTrack track, bool restartIfSame = false)
        {
            if (!musicTracks.TryGetValue(track, out Song song))
            {
                return;
            }

            if (!restartIfSame && currentTrack == track && MediaPlayer.State == MediaState.Playing)
            {
                return;
            }

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
            currentTrack = track;
        }

        public void PlayOneShot(SoundEffectId id, float volume = 1.0f)
        {
            if (!soundEffects.TryGetValue(id, out SoundEffect effect))
            {
                return;
            }

            try
            {
                effect.Play(volume, 0.0f, 0.0f);
            }
            catch (InstancePlayLimitException)
            {
            }
        }

        public double GetDurationSeconds(SoundEffectId id)
        {
            if (!soundEffects.TryGetValue(id, out SoundEffect effect)) return 0.0;

            return effect.Duration.TotalSeconds;
        }

        public void EnsureLoop(AudioLoopChannel channel, SoundEffectId id, float volume = 1.0f)
        {
            if (!soundEffects.TryGetValue(id, out SoundEffect effect))
            {
                return;
            }

            if (activeLoopIds.TryGetValue(channel, out SoundEffectId activeId) && activeId != id)
            {
                StopLoop(channel);
            }

            if (!activeLoops.TryGetValue(channel, out SoundEffectInstance instance))
            {
                instance = effect.CreateInstance();
                instance.IsLooped = true;
                activeLoops[channel] = instance;
                activeLoopIds[channel] = id;
            }

            instance.Volume = volume;

            if (instance.State != SoundState.Playing)
            {
                instance.Play();
            }
        }

        public void StopLoop(AudioLoopChannel channel)
        {
            if (!activeLoops.TryGetValue(channel, out SoundEffectInstance instance))
            {
                return;
            }

            instance.Stop();
            instance.Dispose();
            activeLoops.Remove(channel);
            activeLoopIds.Remove(channel);
        }

        public void StopAllLoops()
        {
            foreach (AudioLoopChannel channel in activeLoops.Keys)
            {
                activeLoops[channel].Stop();
                activeLoops[channel].Dispose();
            }

            activeLoops.Clear();
            activeLoopIds.Clear();
        }

        private static void TryLoad<TAsset>(ContentManager content, string assetName, MusicTrack track, Dictionary<MusicTrack, TAsset> assets)
        {
            try
            {
                assets[track] = content.Load<TAsset>(assetName);
            }
            catch (ContentLoadException)
            {
            }
        }

        private static void TryLoad<TAsset>(ContentManager content, string assetName, SoundEffectId soundEffectId, Dictionary<SoundEffectId, TAsset> assets)
        {
            try
            {
                assets[soundEffectId] = content.Load<TAsset>(assetName);
            }
            catch (ContentLoadException)
            {
            }
        }
    }
}