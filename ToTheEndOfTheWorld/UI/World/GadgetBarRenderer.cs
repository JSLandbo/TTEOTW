using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Grids;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.Inventory;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.World
{
    public sealed class GadgetBarRenderer(WorldElementsRepository blocks, GameItemsRepository items)
    {
        private const float KeyLabelScale = 1.0f;

        private readonly ItemTextureResolver textureResolver = new(blocks, items);
        private ItemSlotRenderer slotRenderer = null!;
        private Texture2D pixelTexture = null!;
        private SpriteFont textFont = null!;

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData([Color.White]);
            textFont = content.Load<SpriteFont>("File");
            slotRenderer = new ItemSlotRenderer(textureResolver, pixelTexture, textFont);
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight, Point mousePosition, InventoryOverlay? inventoryOverlay)
        {
            if (!world.Player.HasGadgetBelt)
            {
                return;
            }

            bool isInteractive = inventoryOverlay?.IsOpen == true;

            for (int slotIndex = 0; slotIndex < GadgetBarLayout.TotalSlotCount; slotIndex++)
            {
                Rectangle slotRectangle = GadgetBarLayout.GetSlotRectangle(viewportWidth, viewportHeight, slotIndex);
                AGridBox slot = world.Player.GadgetSlots.Items.InternalGrid[slotIndex, 0];

                Color backgroundColor = slotIndex < GadgetBarLayout.HotbarSlotCount
                    ? new Color(42, 42, 42, 220)
                    : new Color(34, 34, 34, 220);
                Color borderColor = new(132, 132, 132);
                bool hasHeldItem = inventoryOverlay?.HasHeldItem == true;

                bool isHovered = isInteractive
                    && slotRectangle.Contains(mousePosition)
                    && UiSlotInteractionHelper.CanInteractWithSlot(slot, hasHeldItem)
                    && (inventoryOverlay == null || inventoryOverlay.CanPlaceHeldItemInGadgetSlot(world.Player.GadgetSlots, slotIndex));
                slotRenderer.DrawGridSlot(spriteBatch, slotRectangle, slot, backgroundColor, borderColor, isHovered: isHovered);

                if (slotIndex < GadgetBarLayout.HotbarSlotCount)
                {
                    DrawKeyLabel(spriteBatch, (slotIndex + 1).ToString(), slotRectangle);
                }
            }
        }

        private void DrawKeyLabel(SpriteBatch spriteBatch, string text, Rectangle slotRectangle)
        {
            Vector2 position = new(slotRectangle.X + 5, slotRectangle.Y + 3);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, text, position, new Color(232, 232, 232), KeyLabelScale);
        }
    }
}
