using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Types;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class DynamiteConsumeableService(WorldBlockDamageService worldBlockDamageService, WorldEffectsRepository worldEffects, GameEventBus eventBus)
    {
        public bool TryUse(ModelWorld world, ADynamite dynamite)
        {
            Vector2 center = PlayerWorldPositionService.GetPlayerWorldPosition(world);
            int halfExtent = dynamite.ExplosionAreaSize / 2;
            eventBus.Publish(new ConsumeableUsedEvent(dynamite));

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

            eventBus.Publish(new ConsumeableUsedEvent(dynamite));
            return true;
        }
    }
}