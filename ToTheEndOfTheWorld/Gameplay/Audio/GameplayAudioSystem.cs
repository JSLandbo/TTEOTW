using System;
using ModelLibrary.Abstract.Types;
using ToTheEndOfTheWorld.Context;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Audio
{
    public sealed class GameplayAudioSystem
    {
        private readonly GameAudioService audioService;

        private double currentTotalSeconds;
        private double lastExplosionPlayedAt = double.MinValue;
        private double lastMinedBlockPlayedAt = double.MinValue;

        public GameplayAudioSystem(GameAudioService audioService, GameEventBus eventBus)
        {
            this.audioService = audioService;
            eventBus.Subscribe<ExplosionTriggeredEvent>(OnExplosionTriggered);
            eventBus.Subscribe<WorldBlockDestroyedEvent>(OnWorldBlockDestroyed);
            eventBus.Subscribe<ShopTransactionEvent>(OnShopTransaction);
            eventBus.Subscribe<ConsumeableUsedEvent>(OnConsumeableUsed);
            eventBus.Subscribe<TrashBinUsedEvent>(OnTrashBinUsed);
            eventBus.Subscribe<PlayerSelfDestructedEvent>(OnPlayerSelfDestructed);
            eventBus.Subscribe<PlayerFallDamageEvent>(OnPlayerFallDamage);
            eventBus.Subscribe<PlayerCraftedItemEvent>(OnPlayerCraftedItem);
            eventBus.Subscribe<ScreenEffectRequestedEvent>(OnScreenEffectRequested);
        }

        public void SetTime(double totalSeconds)
        {
            this.currentTotalSeconds = totalSeconds;
        }

        public void Update(ModelWorld world, bool isGrounded)
        {
            if (world.Player.Mining)
            {
                audioService.EnsureLoop(AudioLoopChannel.Mining, SoundEffectId.LoopMining);
            }
            else
            {
                audioService.StopLoop(AudioLoopChannel.Mining);
            }

            if (isGrounded && Math.Abs(world.Player.XVelocity) > PlayerWorldTuning.VelocityStopThreshold)
            {
                audioService.EnsureLoop(AudioLoopChannel.Engine, SoundEffectId.LoopEngine);
            }
            else
            {
                audioService.StopLoop(AudioLoopChannel.Engine);
            }

            if (PlayerThrusterUsageService.UsesThrustersForMovement(world.Player, isGrounded))
            {
                audioService.EnsureLoop(AudioLoopChannel.Thruster, SoundEffectId.LoopThruster);
            }
            else
            {
                audioService.StopLoop(AudioLoopChannel.Thruster);
            }
        }

        public void StopAllLoops()
        {
            audioService.StopAllLoops();
        }

        private void OnExplosionTriggered(ExplosionTriggeredEvent gameEvent)
        {
            TryPlayOneShot(SoundEffectId.EffectExplosion, ref lastExplosionPlayedAt, 0.05);
        }

        private void OnWorldBlockDestroyed(WorldBlockDestroyedEvent gameEvent)
        {
            if (gameEvent.Method == WorldBlockDestroyMethod.Mined)
            {
                TryPlayOneShot(SoundEffectId.EffectMinedBlock, ref lastMinedBlockPlayedAt, 0.08);
            }
        }

        private void OnShopTransaction(ShopTransactionEvent gameEvent)
        {
            audioService.PlayOneShot(gameEvent.Type == ShopTransactionType.Bought
                ? SoundEffectId.EffectBoughtFromStore
                : SoundEffectId.EffectSoldToStore
            );
        }

        private void OnConsumeableUsed(ConsumeableUsedEvent gameEvent)
        {
            if (gameEvent.Consumeable is AFuelCapsule)
            {
                audioService.PlayOneShot(SoundEffectId.EffectUsedFuelCapsule);
                return;
            }

            if (gameEvent.Consumeable is ACoolantPatch)
            {
                audioService.PlayOneShot(SoundEffectId.EffectCoolantPatch);
                return;
            }

            if (gameEvent.Consumeable is AHullRepairKit)
            {
                audioService.PlayOneShot(SoundEffectId.EffectHullRepairKit);
            }
        }

        private void OnTrashBinUsed(TrashBinUsedEvent gameEvent)
        {
            audioService.PlayOneShot(SoundEffectId.EffectUsedTrashBin);
        }

        private void OnPlayerSelfDestructed(PlayerSelfDestructedEvent gameEvent)
        {
            audioService.PlayOneShot(SoundEffectId.EffectYouDied);
        }

        private void OnPlayerFallDamage(PlayerFallDamageEvent gameEvent)
        {
            audioService.PlayOneShot(SoundEffectId.EffectHittingGround);
        }

        private void OnPlayerCraftedItem(PlayerCraftedItemEvent gameEvent)
        {
            audioService.PlayOneShot(SoundEffectId.EffectCraftedItem);
        }

        private void OnScreenEffectRequested(ScreenEffectRequestedEvent gameEvent)
        {
            if (gameEvent.Type == ScreenEffectType.Explosion)
            {
                audioService.PlayOneShot(SoundEffectId.EffectExplosion);
            }
        }

        private void TryPlayOneShot(SoundEffectId id, ref double lastPlayedAt, double minimumReplayIntervalSeconds, float volume = 1.0f)
        {
            if (currentTotalSeconds - lastPlayedAt < minimumReplayIntervalSeconds)
            {
                return;
            }

            audioService.PlayOneShot(id, volume);

            lastPlayedAt = currentTotalSeconds;
        }
    }
}
