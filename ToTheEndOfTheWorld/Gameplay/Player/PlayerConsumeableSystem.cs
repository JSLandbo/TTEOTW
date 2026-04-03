using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Items;
using ToTheEndOfTheWorld.Gameplay.Audio;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerConsumeableSystem(WorldBlockDamageService worldBlockDamageService, WorldEffectsRepository worldEffects, GameEventBus eventBus)
    {
        public void TryUse(ModelWorld world, int slotIndex)
        {
            if (!world.Player.HasGadgetBelt)
            {
                return;
            }

            AGridBox slot = world.Player.GadgetSlots.Items.InternalGrid[slotIndex, 0];

            if (slot.Item is not AConsumeable consumeable || slot.Count <= 0)
            {
                return;
            }

            if (UseConsumeable(world, consumeable))
            {
                Consume(slot);
            }
        }

        private bool UseConsumeable(ModelWorld world, AConsumeable consumeable)
        {
            if (consumeable is SmallDynamite dynamite)
            {
                UseSmallDynamite(world, dynamite);
                return true;
            }

            return false;
        }

        private void UseSmallDynamite(ModelWorld world, SmallDynamite dynamite)
        {
            Vector2 center = PlayerWorldPositionService.GetPlayerWorldPosition(world);
            int halfExtent = dynamite.ExplosionAreaSize / 2;
            eventBus.Publish(new ExplosionTriggeredEvent(new WorldTile((long)center.X, (long)center.Y)));

            for (int y = -halfExtent; y <= halfExtent; y++)
            {
                for (int x = -halfExtent; x <= halfExtent; x++)
                {
                    Vector2 targetVector = new(center.X + x, center.Y + y);

                    if (!worldBlockDamageService.TryDamageBlock(world, targetVector, dynamite.Damage, dynamite.MaxHardness, WorldBlockDestroyMethod.Destroyed, out _))
                    {
                        continue;
                    }

                    worldEffects.Add(new WorldEffect(WorldEffectType.Explosion, new WorldTile((long)targetVector.X, (long)targetVector.Y), dynamite.ExplosionPlaybackFrames));
                }
            }
        }

        private static void Consume(AGridBox slot)
        {
            slot.Count -= 1;

            if (slot.Count > 0)
            {
                return;
            }

            slot.Item = null;
            slot.Count = 0;
        }
    }
}
