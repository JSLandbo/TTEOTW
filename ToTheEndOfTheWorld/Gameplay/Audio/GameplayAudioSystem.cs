using System;
using System.Collections.Generic;
using ModelLibrary.Abstract.Types;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Audio
{
    public sealed class GameplayAudioSystem
    {
        private readonly GameAudioService audioService;
        private readonly ConcurrentOneShotSoundEffectPlayer oneShotSoundPlayer;

        public GameplayAudioSystem(GameAudioService audioService, GameEventBus eventBus)
        {
            this.audioService = audioService;

            oneShotSoundPlayer = new ConcurrentOneShotSoundEffectPlayer(audioService, 2, new Dictionary<SoundEffectId, int>
            {
                [SoundEffectId.EffectExplosion] = 4,
                [SoundEffectId.EffectHullDamage] = 1,
                [SoundEffectId.EffectMinedBlock] = 1,
                [SoundEffectId.EffectBoughtFromStore] = 25,
                [SoundEffectId.EffectSoldToStore] = 25
            });

            eventBus.Subscribe<WorldBlockDestroyedEvent>(OnWorldBlockDestroyed);
            eventBus.Subscribe<ShopTransactionEvent>(OnShopTransaction);
            eventBus.Subscribe<ConsumeableUsedEvent>(OnConsumeableUsed);
            eventBus.Subscribe<TrashBinUsedEvent>(OnTrashBinUsed);
            eventBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
            eventBus.Subscribe<PlayerFallDamageEvent>(OnPlayerFallDamage);
            eventBus.Subscribe<PlayerCraftedItemEvent>(OnPlayerCraftedItem);
            eventBus.Subscribe<PlayerHullDamagedEvent>(OnPlayerHullDamaged);
            eventBus.Subscribe<ScreenEffectRequestedEvent>(OnScreenEffectRequested);
            eventBus.Subscribe<InventoryFullEvent>(OnInventoryFull);
        }

        public void Update(ModelWorld world, bool isGrounded)
        {
            if (world.Player.DrillExtended)
            {
                audioService.EnsureLoop(AudioLoopChannel.Mining, SoundEffectId.LoopMining);
            }
            else
            {
                audioService.StopLoop(AudioLoopChannel.Mining);
            }

            if (isGrounded && Math.Abs(world.Player.XVelocity) > PlayerWorldTuning.VelocityStopThreshold)
            {
                audioService.EnsureLoop(AudioLoopChannel.Engine, SoundEffectId.LoopEngineActive);
            }
            else
            {
                audioService.StopLoop(AudioLoopChannel.Engine);
            }

            if (PlayerThrusterUsageService.UsesThrustersForMovement(world.Player, isGrounded))
            {
                audioService.EnsureLoop(AudioLoopChannel.Thruster, SoundEffectId.LoopThrusterActive);
            }
            else
            {
                audioService.StopLoop(AudioLoopChannel.Thruster);
            }
        }

        public void StopAllLoops()
        {
            audioService.StopAllLoops();
            oneShotSoundPlayer.Clear();
        }

        private void OnWorldBlockDestroyed(WorldBlockDestroyedEvent gameEvent)
        {
            if (gameEvent.Method == WorldBlockDestroyMethod.Mined)
            {
                oneShotSoundPlayer.Play(SoundEffectId.EffectMinedBlock);
            }
        }

        private void OnShopTransaction(ShopTransactionEvent gameEvent)
        {
            oneShotSoundPlayer.Play(gameEvent.Type == ShopTransactionType.Bought
                ? SoundEffectId.EffectBoughtFromStore
                : SoundEffectId.EffectSoldToStore
            );
        }

        private void OnConsumeableUsed(ConsumeableUsedEvent gameEvent)
        {
            if (gameEvent.Consumeable is AFuelCapsule)
            {
                oneShotSoundPlayer.Play(SoundEffectId.EffectUsedFuelCapsule);
                return;
            }

            if (gameEvent.Consumeable is ACoolantPatch)
            {
                oneShotSoundPlayer.Play(SoundEffectId.EffectCoolantPatch);
                return;
            }

            if (gameEvent.Consumeable is AHullRepairKit)
            {
                oneShotSoundPlayer.Play(SoundEffectId.EffectHullRepairKit);
                return;
            }

            if (gameEvent.Consumeable is ADynamite dynamite)
            {
                if (dynamite.ExplosionType.Equals(ExplosionType.Nuclear))
                {
                    oneShotSoundPlayer.Play(SoundEffectId.EffectNuclearExplosion);
                    return;
                }
                oneShotSoundPlayer.Play(SoundEffectId.EffectExplosion);
            }
        }

        private void OnTrashBinUsed(TrashBinUsedEvent gameEvent)
        {
            oneShotSoundPlayer.Play(SoundEffectId.EffectUsedTrashBin);
        }

        private void OnPlayerDied(PlayerDiedEvent gameEvent)
        {
            oneShotSoundPlayer.Play(SoundEffectId.EffectYouDied);
        }

        private void OnPlayerFallDamage(PlayerFallDamageEvent gameEvent)
        {
            oneShotSoundPlayer.Play(SoundEffectId.EffectHittingGround);
        }

        private void OnPlayerCraftedItem(PlayerCraftedItemEvent gameEvent)
        {
            oneShotSoundPlayer.Play(SoundEffectId.EffectCraftedItem);
        }

        private void OnPlayerHullDamaged(PlayerHullDamagedEvent gameEvent)
        {
            oneShotSoundPlayer.Play(SoundEffectId.EffectHullDamage);
        }

        private void OnInventoryFull(InventoryFullEvent gameEvent)
        {
            oneShotSoundPlayer.Play(SoundEffectId.EffectInventoryFull);
        }

        private void OnScreenEffectRequested(ScreenEffectRequestedEvent gameEvent)
        {
            if (gameEvent.Type == ScreenEffectType.Explosion)
            {
                oneShotSoundPlayer.Play(SoundEffectId.EffectExplosion);
            }
        }
    }
}
