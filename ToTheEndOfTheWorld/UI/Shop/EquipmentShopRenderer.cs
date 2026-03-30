using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete;
using System;
using ToTheEndOfTheWorld.Context;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.Inventory;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class EquipmentShopRenderer
    {
        private const float TitleTextScale = 1.35f;
        private const float BodyTextScale = 1.2f;
        private const float PriceTextScale = 1.1f;
        private const float FooterTextScale = 1.1f;

        private readonly InventoryItemTextureResolver textureResolver;
        private ItemSlotRenderer slotRenderer = null!;
        private Texture2D pixelTexture = null!;
        private SpriteFont textFont = null!;

        public EquipmentShopRenderer(WorldElementsRepository blocks, GameItemsRepository items)
        {
            textureResolver = new InventoryItemTextureResolver(blocks, items);
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White });
            textFont = content.Load<SpriteFont>("Fonts/text");
            slotRenderer = new ItemSlotRenderer(textureResolver, pixelTexture, textFont);
        }

        public void Draw(SpriteBatch spriteBatch, World world, ABuilding building, EquipmentShopLayout layout, int viewportWidth, int viewportHeight, Point mousePosition)
        {
            var grid = building.StorageGrid.InternalGrid;

            spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, viewportWidth, viewportHeight), Color.Black * 0.45f);
            spriteBatch.Draw(pixelTexture, new Rectangle(layout.PanelRectangle.X + 3, layout.PanelRectangle.Y + 4, layout.PanelRectangle.Width, layout.PanelRectangle.Height), new Color(0, 0, 0, 70));
            spriteBatch.Draw(pixelTexture, layout.PanelRectangle, new Color(22, 22, 22));
            spriteBatch.Draw(pixelTexture, layout.HeaderRectangle, new Color(44, 44, 44));
            DrawRectangleOutline(spriteBatch, layout.PanelRectangle, 2, new Color(108, 108, 108));

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, building.Name, new Vector2(layout.PanelRectangle.X + EquipmentShopLayout.TitlePaddingLeft, layout.PanelRectangle.Y + EquipmentShopLayout.TitlePaddingTop - 2), new Color(244, 240, 229), TitleTextScale);
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    var slotRectangle = layout.GetSlotRectangle(x, y);
                    DrawSlot(spriteBatch, world, grid[x, y], slotRectangle, slotRectangle.Contains(mousePosition));
                }
            }

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Press E or Escape to close", new Vector2(layout.PanelRectangle.X + EquipmentShopLayout.TitlePaddingLeft, layout.PanelRectangle.Bottom - EquipmentShopLayout.FooterTextBottomPadding), new Color(188, 188, 188), FooterTextScale);
        }

        private void DrawSlot(SpriteBatch spriteBatch, World world, AGridBox slot, Rectangle slotRectangle, bool isHovered)
        {
            var canAfford = slot.Item != null && world.Player.Cash >= slot.Item.Worth;
            var backgroundColor = canAfford ? new Color(74, 62, 38) : new Color(40, 40, 40);
            var borderColor = canAfford ? new Color(208, 180, 96) : new Color(82, 82, 82);
            slotRenderer.DrawGridSlot(spriteBatch, slotRectangle, slot, backgroundColor, borderColor, showCount: false, isHovered: isHovered);

            if (!canAfford)
            {
                spriteBatch.Draw(pixelTexture, slotRectangle, Color.Black * 0.18f);
            }

            if (slot.Item != null)
            {
                DrawPrice(spriteBatch, slot.Item, slotRectangle, canAfford, isHovered);
            }
        }

        private void DrawPrice(SpriteBatch spriteBatch, AType item, Rectangle slotRectangle, bool canAfford, bool isHovered)
        {
            var priceText = Math.Floor(item.Worth).ToString();
            var priceSize = textFont.MeasureString(priceText) * PriceTextScale;
            var badgeRectangle = new Rectangle(
                slotRectangle.Right - (int)priceSize.X - 16,
                slotRectangle.Y + 4,
                (int)priceSize.X + 10,
                (int)priceSize.Y + 6);

            spriteBatch.Draw(pixelTexture, badgeRectangle, canAfford ? new Color(112, 92, 48) : new Color(46, 46, 46));
            DrawRectangleOutline(spriteBatch, badgeRectangle, 1, canAfford ? (isHovered ? new Color(250, 226, 136) : new Color(198, 176, 108)) : new Color(98, 98, 98));
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, priceText, new Vector2(badgeRectangle.X + 5, badgeRectangle.Y + 2), canAfford ? new Color(244, 230, 190) : new Color(194, 194, 194), PriceTextScale);
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
