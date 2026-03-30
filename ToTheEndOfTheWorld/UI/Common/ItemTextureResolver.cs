using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Blocks;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.UI.Common
{
    public sealed class ItemTextureResolver
    {
        private readonly WorldElementsRepository blocks;
        private readonly GameItemsRepository items;

        public ItemTextureResolver(WorldElementsRepository blocks, GameItemsRepository items)
        {
            this.blocks = blocks;
            this.items = items;
        }

        public Texture2D Resolve(AType item)
        {
            if (item is Block block && blocks.TryGetValue(block.ID, out (string Name, Texture2D Texture, Block block) blockDefinition))
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

            return null;
        }
    }
}
