using Microsoft.Xna.Framework.Graphics;
using System;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Blocks;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.UI.Common
{
    public sealed class ItemTextureResolver(WorldElementsRepository blocks, GameItemsRepository items)
    {
        public Texture2D Resolve(AType item)
        {
            if (item is Block block && blocks.TryGetValue(block.ID, out (string Name, Texture2D Texture, int Frames, Block block) blockDefinition))
            {
                return blockDefinition.Texture;
            }

            if (items.TryGetValue(item.ID, out GameItemDefinition itemDefinition))
            {
                if (itemDefinition.Textures.TryGetValue(PlayerOrientation.Base, out Texture2D baseTexture))
                {
                    return baseTexture;
                }

                foreach (Texture2D texture in itemDefinition.Textures.Values)
                {
                    return texture;
                }
            }

            throw new InvalidOperationException($"No texture registered for item '{item.Name}' ({item.ID}).");
        }

        public int ResolveFrames(AType item)
        {
            if (item is Block block && blocks.TryGetValue(block.ID, out (string Name, Texture2D Texture, int Frames, Block block) blockDefinition))
            {
                return blockDefinition.Frames;
            }

            if (items.TryGetValue(item.ID, out GameItemDefinition itemDefinition))
            {
                return itemDefinition.Frames;
            }

            return 1;
        }
    }
}
