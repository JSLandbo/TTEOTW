using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Enums;
using System;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class FuelStationOverlay : IInteractionOverlay
    {
        private const int PanelWidth = 460;
        private const int PanelHeight = 240;
        private const int ButtonWidth = 260;
        private const int ButtonHeight = 62;
        private const float TitleTextScale = 1.35f;
        private const float BodyTextScale = 1.2f;
        private const float ButtonTextScale = 1.3f;

        private Texture2D pixelTexture = null!;
        private SpriteFont textFont = null!;
        private bool isOpen;

        public EBuildingInteraction Action => EBuildingInteraction.FuelStation;
        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;

        public void Open(ABuilding building)
        {
            isOpen = true;
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData([Color.White]);
            textFont = content.Load<SpriteFont>("File");
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            if ((currentKeyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape)) ||
                (currentKeyboardState.IsKeyDown(Keys.E) && !previousKeyboardState.IsKeyDown(Keys.E)))
            {
                isOpen = false;
                return;
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed &&
                previousMouseState.LeftButton == ButtonState.Released &&
                GetRefuelButtonRectangle(viewportWidth, viewportHeight).Contains(currentMouseState.Position))
            {
                RefuelAllAffordable(world);
            }
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            Rectangle panelRectangle = new((viewportWidth - PanelWidth) / 2, (viewportHeight - PanelHeight) / 2, PanelWidth, PanelHeight);
            Rectangle refuelButtonRectangle = GetRefuelButtonRectangle(viewportWidth, viewportHeight);
            float missingFuel = world.Player.FuelTank.Capacity - world.Player.FuelTank.Fuel;
            float affordableFuel = MathF.Min(missingFuel, (float)world.Player.Cash);
            bool canRefuel = affordableFuel > 0.0f;

            spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, viewportWidth, viewportHeight), Color.Black * 0.45f);
            spriteBatch.Draw(pixelTexture, panelRectangle, new Color(22, 22, 22));
            spriteBatch.Draw(pixelTexture, refuelButtonRectangle, canRefuel ? new Color(86, 110, 78) : new Color(64, 64, 64));

            DrawRectangleOutline(spriteBatch, panelRectangle, 2, new Color(108, 108, 108));
            DrawRectangleOutline(spriteBatch, refuelButtonRectangle, 2, canRefuel ? new Color(152, 182, 140) : new Color(110, 110, 110));

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Fuel Station", new Vector2(panelRectangle.X + 20, panelRectangle.Y + 12), new Color(244, 240, 229), TitleTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Fuel: {world.Player.FuelTank.Fuel:0.00} / {world.Player.FuelTank.Capacity:0.00}", new Vector2(panelRectangle.X + 22, panelRectangle.Y + 72), new Color(230, 230, 230), BodyTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Can Buy: {affordableFuel:0.00}", new Vector2(panelRectangle.X + 22, panelRectangle.Y + 108), new Color(214, 214, 214), BodyTextScale);
            DrawCenteredText(spriteBatch, "Refuel!", refuelButtonRectangle, new Color(248, 243, 233), ButtonTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Press E or Escape to close", new Vector2(panelRectangle.X + 20, panelRectangle.Bottom - 40), new Color(188, 188, 188), BodyTextScale);
        }

        private static float RefuelAllAffordable(ModelWorld world)
        {
            float missingFuel = world.Player.FuelTank.Capacity - world.Player.FuelTank.Fuel;

            if (missingFuel <= 0.0f || world.Player.Cash <= 0.0)
            {
                return 0.0f;
            }

            float fuelPurchased = MathF.Min(missingFuel, (float)world.Player.Cash);
            world.Player.FuelTank.Fuel += fuelPurchased;
            world.Player.Cash -= fuelPurchased;
            return fuelPurchased;
        }

        private Rectangle GetRefuelButtonRectangle(int viewportWidth, int viewportHeight)
        {
            return new Rectangle((viewportWidth - ButtonWidth) / 2, (viewportHeight - PanelHeight) / 2 + 146, ButtonWidth, ButtonHeight);
        }

        private void DrawCenteredText(SpriteBatch spriteBatch, string text, Rectangle rectangle, Color color, float scale)
        {
            Vector2 size = textFont.MeasureString(text) * scale;
            Vector2 position = new(
                rectangle.X + ((rectangle.Width - size.X) / 2f),
                rectangle.Y + ((rectangle.Height - size.Y) / 2f));
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, text, position, color, scale);
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
