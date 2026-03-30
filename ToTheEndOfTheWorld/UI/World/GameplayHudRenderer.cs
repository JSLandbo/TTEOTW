using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.World
{
    public sealed class GameplayHudRenderer
    {
        private Texture2D pixelTexture;
        private SpriteFont textFont;

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White });
            textFont = content.Load<SpriteFont>("Fonts/text");
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, InventoryService inventoryService, int viewportWidth)
        {
            DrawPlayerHud(spriteBatch, world, inventoryService, viewportWidth);
        }

        private void DrawPlayerHud(SpriteBatch spriteBatch, ModelWorld world, InventoryService inventoryService, int viewportWidth)
        {
            var player = world.Player;
            var moneyText = $"Money: {Math.Floor(player.Cash)}";
            var capacityText = $"Inventory: {inventoryService.GetUsedCapacityPercent(player.Inventory)}%";
            const float hudScale = 1.35f;
            const int cardPaddingX = 14;
            const int cardPaddingY = 10;
            const int cardSpacing = 10;
            const int topMargin = 10;
            const int rightMargin = 10;

            var moneySize = textFont.MeasureString(moneyText) * hudScale;
            var capacitySize = textFont.MeasureString(capacityText) * hudScale;

            var moneyRectangle = new Rectangle(
                0,
                topMargin,
                (int)moneySize.X + (cardPaddingX * 2),
                (int)moneySize.Y + (cardPaddingY * 2));

            var capacityRectangle = new Rectangle(
                0,
                moneyRectangle.Bottom + cardSpacing,
                (int)capacitySize.X + (cardPaddingX * 2),
                (int)capacitySize.Y + (cardPaddingY * 2));

            var cardWidth = Math.Max(moneyRectangle.Width, capacityRectangle.Width);
            moneyRectangle.Width = cardWidth;
            capacityRectangle.Width = cardWidth;
            moneyRectangle.X = viewportWidth - cardWidth - rightMargin;
            capacityRectangle.X = viewportWidth - cardWidth - rightMargin;

            DrawHudCard(spriteBatch, moneyRectangle, new Color(24, 24, 24, 205), new Color(118, 118, 118, 220));
            DrawHudCard(spriteBatch, capacityRectangle, new Color(30, 30, 30, 205), new Color(104, 104, 104, 220));

            var moneyPosition = new Vector2(moneyRectangle.Right - cardPaddingX - moneySize.X, moneyRectangle.Y + cardPaddingY);
            var capacityPosition = new Vector2(capacityRectangle.Right - cardPaddingX - capacitySize.X, capacityRectangle.Y + cardPaddingY);

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, moneyText, moneyPosition, new Color(242, 239, 230), hudScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, capacityText, capacityPosition, new Color(230, 230, 230), hudScale);
        }

        private void DrawHudCard(SpriteBatch spriteBatch, Rectangle rectangle, Color backgroundColor, Color borderColor)
        {
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X + 2, rectangle.Y + 3, rectangle.Width, rectangle.Height), new Color(0, 0, 0, 70));
            spriteBatch.Draw(pixelTexture, rectangle, backgroundColor);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1), borderColor);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Bottom - 1, rectangle.Width, 1), new Color(42, 42, 42, 220));
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, 1, rectangle.Height), borderColor);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.Right - 1, rectangle.Y, 1, rectangle.Height), new Color(42, 42, 42, 220));
        }
    }
}
