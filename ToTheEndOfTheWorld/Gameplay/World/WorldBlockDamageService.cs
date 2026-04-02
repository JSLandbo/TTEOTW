using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete.Blocks;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class WorldBlockDamageService(WorldBlockDefinitionResolver worldBlockDefinitionResolver, WorldBlockFactory worldBlockFactory, MiningInteractionsRepository interactions, GameEventBus eventBus)
    {
        public bool TryGetBlockInteraction(ModelWorld world, Vector2 targetVector, out MiningInteraction interaction)
        {
            return TryGetBlockInteraction(world, targetVector, out interaction, out _);
        }

        public bool TryGetBlockInteraction(ModelWorld world, Vector2 targetVector, out MiningInteraction interaction, out bool isBlockedByBuilding)
        {
            WorldTile worldTile = new((long)targetVector.X, (long)targetVector.Y);
            isBlockedByBuilding = IsInsideBuilding(world, worldTile);

            if (isBlockedByBuilding || !worldBlockDefinitionResolver.IsObstructed(world, targetVector))
            {
                interaction = null;
                return false;
            }

            if (!interactions.TryGet(worldTile, out interaction))
            {
                Block block = worldBlockFactory.CreateMutableWorldBlock(targetVector.X, targetVector.Y);
                MiningInteraction createdInteraction = new(new WorldTileBounds(worldTile.X, worldTile.Y, 1, 1), block);
                interactions.Add(createdInteraction);
                interaction = createdInteraction;
            }

            return true;
        }

        public bool TryDamageBlock(ModelWorld world, Vector2 targetVector, float damage, float maxHardness, WorldBlockDestroyMethod destroyMethod, out Block block)
        {
            if (!TryGetBlockInteraction(world, targetVector, out MiningInteraction interaction) || interaction.Block.Hardness > maxHardness)
            {
                block = null;
                return false;
            }

            bool willBeDestroyed = !interaction.Block.Ethereal && interaction.Block.CurrentHealth > 0 && interaction.Block.CurrentHealth <= damage;
            interaction.Block.TakeDamage(damage);

            if (willBeDestroyed)
            {
                OnBlockDestroyed(world, interaction, destroyMethod);
            }

            block = interaction.Block;
            return true;
        }

        private void OnBlockDestroyed(ModelWorld world, MiningInteraction interaction, WorldBlockDestroyMethod destroyMethod)
        {
            Vector2 location = new(interaction.TileBounds.X, interaction.TileBounds.Y);
            world.WorldTrails.Add(location, true);
            interactions.Remove(interaction);
            eventBus.Publish(new WorldBlockDestroyedEvent(world, interaction.Block.ID, new WorldTile(interaction.TileBounds.X, interaction.TileBounds.Y), destroyMethod));
        }

        private static bool IsInsideBuilding(ModelWorld world, WorldTile tile)
        {
            if (world.Buildings == null)
            {
                return false;
            }

            foreach (ABuilding building in world.Buildings)
            {
                if (building.ContainsTile(tile.X, tile.Y))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
