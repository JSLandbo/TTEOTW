using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Concrete.Blocks;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class WorldBlockFactory(WorldBlockDefinitionResolver worldBlockDefinitionResolver)
    {
        public Block CreateMutableWorldBlock(float x, float y)
        {
            KeyValuePair<int, (string Name, Texture2D Texture, Block block)> definition = worldBlockDefinitionResolver.GetWorldBlock(x, y);

            Block block = new(definition.Value.block);

            // TODO: Key 2 is dirt, consider other ores as well!
            if (definition.Key == 2 && y > 0)
            {
                block.CurrentHealth += 0.001f * y;
                block.MaximumHealth += 0.001f * y;
                block.Hardness += 0.001f * y;
            }

            return block;
        }
    }
}