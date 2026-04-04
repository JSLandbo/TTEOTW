using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract;
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
            pixelTexture.SetData([Color.White]);
            textFont = content.Load<SpriteFont>("File");
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, InventoryService inventoryService, int viewportWidth)
        {
            DrawPlayerHud(spriteBatch, world, inventoryService, viewportWidth);
        }

        private void DrawPlayerHud(SpriteBatch spriteBatch, ModelWorld world, InventoryService inventoryService, int viewportWidth)
        {
            APlayer player = world.Player;
            string moneyText = $"Money: {Math.Floor(player.Cash)}";
            string fuelText = $"Fuel: {player.CurrentFuel:0.00}/{player.FuelTank.Capacity:0.00}";
            string heatText = $"Heat: {player.CurrentHeat:0.00}/{player.ThermalPlating.MaxThermals:0.00}";
            string hullText = $"Hull: {player.CurrentHull:0}";
            string weightText = $"Weight: {player.Weight:0.##}/{player.Thruster.Power:0.##}";
            string slotsText = $"Slots: {inventoryService.GetUsedSlotsPercent(player.Inventory)}%";
            const float hudScale = 1.05f;
            const int cardPaddingX = 14;
            const int cardPaddingY = 10;
            const int cardSpacing = 10;
            const int topMargin = 10;
            const int rightMargin = 10;

            Vector2 moneySize = textFont.MeasureString(moneyText) * hudScale;
            Vector2 fuelSize = textFont.MeasureString(fuelText) * hudScale;
            Vector2 heatSize = textFont.MeasureString(heatText) * hudScale;
            Vector2 hullSize = textFont.MeasureString(hullText) * hudScale;
            Vector2 weightSize = textFont.MeasureString(weightText) * hudScale;
            Vector2 slotsSize = textFont.MeasureString(slotsText) * hudScale;

            Rectangle moneyRectangle = new(
                0,
                topMargin,
                (int)moneySize.X + (cardPaddingX * 2),
                (int)moneySize.Y + (cardPaddingY * 2));

            Rectangle fuelRectangle = new(
                0,
                0,
                (int)fuelSize.X + (cardPaddingX * 2),
                (int)fuelSize.Y + (cardPaddingY * 2));

            Rectangle heatRectangle = new(
                0,
                0,
                (int)heatSize.X + (cardPaddingX * 2),
                (int)heatSize.Y + (cardPaddingY * 2));

            Rectangle hullRectangle = new(
                0,
                0,
                (int)hullSize.X + (cardPaddingX * 2),
                (int)hullSize.Y + (cardPaddingY * 2));

            Rectangle weightRectangle = new(
                0,
                0,
                (int)weightSize.X + (cardPaddingX * 2),
                (int)weightSize.Y + (cardPaddingY * 2));

            Rectangle slotsRectangle = new(
                0,
                0,
                (int)slotsSize.X + (cardPaddingX * 2),
                (int)slotsSize.Y + (cardPaddingY * 2));

            fuelRectangle.Y = moneyRectangle.Bottom + cardSpacing;
            heatRectangle.Y = fuelRectangle.Bottom + cardSpacing;
            hullRectangle.Y = heatRectangle.Bottom + cardSpacing;
            weightRectangle.Y = hullRectangle.Bottom + cardSpacing;
            slotsRectangle.Y = weightRectangle.Bottom + cardSpacing;

            int cardWidth = Math.Max(Math.Max(moneyRectangle.Width, fuelRectangle.Width), Math.Max(heatRectangle.Width, Math.Max(hullRectangle.Width, Math.Max(weightRectangle.Width, slotsRectangle.Width))));

            moneyRectangle.Width = cardWidth;
            fuelRectangle.Width = cardWidth;
            heatRectangle.Width = cardWidth;
            hullRectangle.Width = cardWidth;
            weightRectangle.Width = cardWidth;
            slotsRectangle.Width = cardWidth;
            moneyRectangle.X = viewportWidth - cardWidth - rightMargin;
            fuelRectangle.X = viewportWidth - cardWidth - rightMargin;
            heatRectangle.X = viewportWidth - cardWidth - rightMargin;
            hullRectangle.X = viewportWidth - cardWidth - rightMargin;
            weightRectangle.X = viewportWidth - cardWidth - rightMargin;
            slotsRectangle.X = viewportWidth - cardWidth - rightMargin;

            DrawHudCard(spriteBatch, moneyRectangle, new Color(24, 24, 24, 205), new Color(118, 118, 118, 220));
            DrawHudCard(spriteBatch, fuelRectangle, new Color(32, 32, 32, 205), new Color(132, 118, 78, 220));
            DrawHudCard(spriteBatch, heatRectangle, new Color(34, 30, 30, 205), new Color(176, 112, 92, 220));
            DrawHudCard(spriteBatch, hullRectangle, new Color(38, 28, 28, 205), new Color(172, 94, 94, 220));
            DrawHudCard(spriteBatch, weightRectangle, new Color(30, 32, 38, 205), new Color(112, 136, 182, 220));
            DrawHudCard(spriteBatch, slotsRectangle, new Color(30, 30, 30, 205), new Color(104, 104, 104, 220));

            Vector2 moneyPosition = new(moneyRectangle.Right - cardPaddingX - moneySize.X, moneyRectangle.Y + cardPaddingY);
            Vector2 fuelPosition = new(fuelRectangle.Right - cardPaddingX - fuelSize.X, fuelRectangle.Y + cardPaddingY);
            Vector2 heatPosition = new(heatRectangle.Right - cardPaddingX - heatSize.X, heatRectangle.Y + cardPaddingY);
            Vector2 hullPosition = new(hullRectangle.Right - cardPaddingX - hullSize.X, hullRectangle.Y + cardPaddingY);
            Vector2 weightPosition = new(weightRectangle.Right - cardPaddingX - weightSize.X, weightRectangle.Y + cardPaddingY);
            Vector2 slotsPosition = new(slotsRectangle.Right - cardPaddingX - slotsSize.X, slotsRectangle.Y + cardPaddingY);

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, moneyText, moneyPosition, new Color(242, 239, 230), hudScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, fuelText, fuelPosition, new Color(240, 224, 170), hudScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, heatText, heatPosition, new Color(244, 188, 168), hudScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, hullText, hullPosition, new Color(248, 198, 198), hudScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, weightText, weightPosition, new Color(214, 224, 244), hudScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, slotsText, slotsPosition, new Color(230, 230, 230), hudScale);
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
