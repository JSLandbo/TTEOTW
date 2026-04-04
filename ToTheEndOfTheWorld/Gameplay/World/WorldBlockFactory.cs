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

            if (block.ID == GameIds.Blocks.Dirt)
            {
                block.Hardness += 0.05f * y;
            }

            block.CurrentHealth += 0.025f * y;
            block.MaximumHealth += 0.025f * y;
            
            return block;
        }
    }
}
