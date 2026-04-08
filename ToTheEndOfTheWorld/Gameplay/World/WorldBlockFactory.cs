using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Concrete.Blocks;
using ModelLibrary.Ids;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class WorldBlockFactory(WorldBlockDefinitionResolver worldBlockDefinitionResolver)
    {
        public Block CreateMutableWorldBlock(float x, float y)
        {
            KeyValuePair<int, (string Name, Texture2D Texture, int Frames, Block block)> definition = worldBlockDefinitionResolver.GetWorldBlock(x, y);

            Block block = new(definition.Value.block);
            float depth = y < 12.0f ? 0.0f : y - 12.0f;

            if (block.ID == GameIds.Blocks.Dirt)
            {
                block.Hardness += 0.008f * depth;
            }
            else if (block.ID == GameIds.Blocks.Rock)
            {
                block.Hardness += 0.004f * depth;
            }

            block.CurrentHealth += 0.015f * depth;
            block.MaximumHealth += 0.015f * depth;

            return block;
        }
    }
}
