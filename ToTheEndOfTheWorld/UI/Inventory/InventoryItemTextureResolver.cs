using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Blocks;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.Context.StaticRepositories;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public sealed class InventoryItemTextureResolver
    {
        private readonly WorldElementsRepository blocks;
        private readonly GameItemsRepository items;

        public InventoryItemTextureResolver(WorldElementsRepository blocks, GameItemsRepository items)
        {
            this.blocks = blocks;
            this.items = items;
        }

        public Texture2D Resolve(AType item)
        {
            if (item is Block block && blocks.TryGetValue(block.ID, out var blockDefinition))
            {
                return blockDefinition.Texture;
            }

            if (items.TryGetValue(item.ID, out var itemDefinition))
            {
                if (itemDefinition.Textures.TryGetValue(PlayerOrientation.Base, out var baseTexture))
                {
                    return baseTexture;
                }

                foreach (var texture in itemDefinition.Textures.Values)
                {
                    return texture;
                }
            }

            return null;
        }
    }
}
