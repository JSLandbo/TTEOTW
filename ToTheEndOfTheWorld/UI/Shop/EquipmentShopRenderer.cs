using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class EquipmentShopRenderer(WorldElementsRepository blocks, GameItemsRepository items)
    {
        private const float TitleTextScale = 1.15f;
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

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, ABuilding building, EquipmentShopLayout layout, int viewportWidth, int viewportHeight, Point mousePosition)
        {
            AGridBox[,] grid = building.StorageGrid.InternalGrid;

            if (!building.ShowPlayerInventoryWhenOpen)
            {
                UiDrawHelper.DrawScreenDim(spriteBatch, pixelTexture, viewportWidth, viewportHeight);
            }

            spriteBatch.Draw(pixelTexture, new Rectangle(layout.PanelRectangle.X + 3, layout.PanelRectangle.Y + 4, layout.PanelRectangle.Width, layout.PanelRectangle.Height), UiColors.PanelShadow);
            spriteBatch.Draw(pixelTexture, layout.PanelRectangle, UiColors.PanelBackground);
            spriteBatch.Draw(pixelTexture, layout.HeaderRectangle, UiColors.HeaderBackground);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, layout.PanelRectangle, 2, UiColors.PanelBorder);

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, building.Name, new Vector2(layout.PanelRectangle.X + EquipmentShopLayout.TitlePaddingLeft, layout.PanelRectangle.Y + EquipmentShopLayout.TitlePaddingTop - 2), UiColors.TextTitle, TitleTextScale);
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    Rectangle slotRectangle = layout.GetSlotRectangle(x, y);
                    bool isHovered = grid[x, y].Item != null && world.Player.Cash >= grid[x, y].Item.Worth && slotRectangle.Contains(mousePosition);
                    DrawSlot(spriteBatch, world, grid[x, y], slotRectangle, isHovered);
                }
            }
        }

        private void DrawSlot(SpriteBatch spriteBatch, ModelWorld world, AGridBox slot, Rectangle slotRectangle, bool isHovered)
        {
            if (slot.Item == null)
            {
                slotRenderer.DrawGridSlot(spriteBatch, slotRectangle, slot, UiColors.SlotBackgroundEmpty, UiColors.SlotBorderEmpty, showCount: false, isHovered: isHovered);
                return;
            }

            bool canAfford = slot.Item != null && world.Player.Cash >= slot.Item.Worth;
            Color backgroundColor = canAfford ? UiColors.SlotBackgroundBuyable : UiColors.SlotBackgroundDisabled;
            Color borderColor = canAfford ? UiColors.SlotBorderBuyable : UiColors.SlotBorderDisabled;
            slotRenderer.DrawGridSlot(spriteBatch, slotRectangle, slot, backgroundColor, borderColor, showCount: false, isHovered: isHovered);

            if (!canAfford)
            {
                spriteBatch.Draw(pixelTexture, slotRectangle, Color.Black * 0.9f);
            }
        }

    }
}
