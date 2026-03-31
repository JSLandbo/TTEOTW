using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using System;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class EquipmentShopRenderer(WorldElementsRepository blocks, GameItemsRepository items)
    {
        private const float TitleTextScale = 1.35f;
        private const float PriceTextScale = 1.1f;
        private const float FooterTextScale = 1.1f;

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

            spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, viewportWidth, viewportHeight), Color.Black * 0.45f);
            spriteBatch.Draw(pixelTexture, new Rectangle(layout.PanelRectangle.X + 3, layout.PanelRectangle.Y + 4, layout.PanelRectangle.Width, layout.PanelRectangle.Height), new Color(0, 0, 0, 70));
            spriteBatch.Draw(pixelTexture, layout.PanelRectangle, new Color(22, 22, 22));
            spriteBatch.Draw(pixelTexture, layout.HeaderRectangle, new Color(44, 44, 44));
            DrawRectangleOutline(spriteBatch, layout.PanelRectangle, 2, new Color(108, 108, 108));

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, building.Name, new Vector2(layout.PanelRectangle.X + EquipmentShopLayout.TitlePaddingLeft, layout.PanelRectangle.Y + EquipmentShopLayout.TitlePaddingTop - 2), new Color(244, 240, 229), TitleTextScale);
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    Rectangle priceRectangle = layout.GetPriceRectangle(x, y);
                    Rectangle slotRectangle = layout.GetSlotRectangle(x, y);
                    DrawSlot(spriteBatch, world, grid[x, y], priceRectangle, slotRectangle, slotRectangle.Contains(mousePosition));
                }
            }

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Press E or Escape to close", new Vector2(layout.PanelRectangle.X + EquipmentShopLayout.TitlePaddingLeft, layout.PanelRectangle.Bottom - EquipmentShopLayout.FooterTextBottomPadding), new Color(188, 188, 188), FooterTextScale);
        }

        private void DrawSlot(SpriteBatch spriteBatch, ModelWorld world, AGridBox slot, Rectangle priceRectangle, Rectangle slotRectangle, bool isHovered)
        {
            if (slot.Item == null)
            {
                slotRenderer.DrawGridSlot(spriteBatch, slotRectangle, slot, new Color(34, 34, 34), new Color(76, 76, 76), showCount: false, isHovered: isHovered);
                return;
            }

            bool canAfford = slot.Item != null && world.Player.Cash >= slot.Item.Worth;
            Color backgroundColor = canAfford ? new Color(74, 62, 38) : new Color(10, 10, 10);
            Color borderColor = canAfford ? new Color(208, 180, 96) : new Color(20, 20, 20);
            slotRenderer.DrawGridSlot(spriteBatch, slotRectangle, slot, backgroundColor, borderColor, showCount: false, isHovered: isHovered);

            if (!canAfford)
            {
                spriteBatch.Draw(pixelTexture, slotRectangle, Color.Black * 0.9f);
            }

            DrawPrice(spriteBatch, slot.Item, priceRectangle, canAfford, isHovered);
        }

        private void DrawPrice(SpriteBatch spriteBatch, AType item, Rectangle priceRectangle, bool canAfford, bool isHovered)
        {
            string priceText = Math.Floor(item.Worth).ToString();
            Vector2 priceSize = textFont.MeasureString(priceText) * PriceTextScale;
            spriteBatch.Draw(pixelTexture, priceRectangle, canAfford ? new Color(66, 57, 34) : new Color(8, 8, 8));
            DrawRectangleOutline(spriteBatch, priceRectangle, 1, canAfford ? (isHovered ? new Color(250, 226, 136) : new Color(198, 176, 108)) : new Color(18, 18, 18));
            Vector2 pricePosition = new(
                priceRectangle.Center.X - (priceSize.X / 2f),
                priceRectangle.Y + ((priceRectangle.Height - priceSize.Y) / 2f) - 1);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, priceText, pricePosition, canAfford ? new Color(244, 230, 190) : new Color(42, 42, 42), PriceTextScale);
        }

        private void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rectangle, int thickness, Color color)
        {
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Bottom - thickness, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.Right - thickness, rectangle.Y, thickness, rectangle.Height), color);
        }

    }
}
