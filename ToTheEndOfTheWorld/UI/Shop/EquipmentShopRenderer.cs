using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete;
using System;
using ToTheEndOfTheWorld.Context;
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
        }

        public void Draw(SpriteBatch spriteBatch, World world, ABuilding building, EquipmentShopLayout layout, int viewportWidth, int viewportHeight)
        {
            var grid = building.StorageGrid.InternalGrid;

            spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, viewportWidth, viewportHeight), Color.Black * 0.45f);
            spriteBatch.Draw(pixelTexture, new Rectangle(layout.PanelRectangle.X + 3, layout.PanelRectangle.Y + 4, layout.PanelRectangle.Width, layout.PanelRectangle.Height), new Color(0, 0, 0, 70));
            spriteBatch.Draw(pixelTexture, layout.PanelRectangle, new Color(22, 22, 22));
            spriteBatch.Draw(pixelTexture, layout.HeaderRectangle, new Color(44, 44, 44));
            DrawRectangleOutline(spriteBatch, layout.PanelRectangle, 2, new Color(108, 108, 108));

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, building.Name, new Vector2(layout.PanelRectangle.X + EquipmentShopLayout.TitlePaddingLeft, layout.PanelRectangle.Y + EquipmentShopLayout.TitlePaddingTop - 2), new Color(244, 240, 229), TitleTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Money: {Math.Floor(world.Player.Cash)}", new Vector2(layout.PanelRectangle.X + EquipmentShopLayout.TitlePaddingLeft, layout.PanelRectangle.Y + EquipmentShopLayout.MoneyPaddingTop), new Color(232, 232, 232), BodyTextScale);

            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    var slotRectangle = layout.GetSlotRectangle(x, y);
                    DrawSlot(spriteBatch, world, grid[x, y], slotRectangle);
                }
            }

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Press E or Escape to close", new Vector2(layout.PanelRectangle.X + EquipmentShopLayout.TitlePaddingLeft, layout.PanelRectangle.Bottom - EquipmentShopLayout.FooterTextBottomPadding), new Color(188, 188, 188), FooterTextScale);
        }

        private void DrawSlot(SpriteBatch spriteBatch, World world, AGridBox slot, Rectangle slotRectangle)
        {
            var canAfford = slot.Item != null && world.Player.Cash >= slot.Item.Worth;
            spriteBatch.Draw(pixelTexture, slotRectangle, canAfford ? new Color(62, 62, 62) : new Color(40, 40, 40));
            DrawRectangleOutline(spriteBatch, slotRectangle, 2, canAfford ? new Color(124, 124, 124) : new Color(82, 82, 82));

            if (slot.Item == null)
            {
                return;
            }

            DrawItemTexture(spriteBatch, slot.Item, slotRectangle);
            DrawPrice(spriteBatch, slot.Item, slotRectangle);
        }

        private void DrawItemTexture(SpriteBatch spriteBatch, AType item, Rectangle slotRectangle)
        {
            var texture = textureResolver.Resolve(item);

            if (texture == null)
            {
                return;
            }

            var textureRectangle = FitTextureInside(slotRectangle, texture.Width, texture.Height, 10);
            spriteBatch.Draw(texture, textureRectangle, Color.White);
        }

        private void DrawPrice(SpriteBatch spriteBatch, AType item, Rectangle slotRectangle)
        {
            var priceText = Math.Floor(item.Worth).ToString();
            var priceSize = textFont.MeasureString(priceText) * PriceTextScale;
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, priceText, new Vector2(slotRectangle.Right - priceSize.X - 6, slotRectangle.Y + 4), new Color(234, 220, 180), PriceTextScale);
        }

        private void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rectangle, int thickness, Color color)
        {
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Bottom - thickness, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.Right - thickness, rectangle.Y, thickness, rectangle.Height), color);
        }

        private static Rectangle FitTextureInside(Rectangle bounds, int textureWidth, int textureHeight, int padding)
        {
            var availableWidth = bounds.Width - (padding * 2);
            var availableHeight = bounds.Height - (padding * 2);
            var scale = Math.Min((float)availableWidth / textureWidth, (float)availableHeight / textureHeight);
            var width = (int)(textureWidth * scale);
            var height = (int)(textureHeight * scale);
            var x = bounds.X + ((bounds.Width - width) / 2);
            var y = bounds.Y + ((bounds.Height - height) / 2);
            return new Rectangle(x, y, width, height);
        }
    }
}
