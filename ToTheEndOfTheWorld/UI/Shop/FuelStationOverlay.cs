using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class FuelStationOverlay : IInteractionOverlay
    {
        private const int PanelWidth = 460;
        private const int PanelHeight = 210;
        private const int HeaderHeight = 58;
        private const int ButtonWidth = 260;
        private const int ButtonHeight = 62;
        private const int ContentPadding = 20;
        private const float TitleTextScale = 1.15f;
        private const float BodyTextScale = 1.0f;
        private const float ButtonTextScale = 1.0f;

        private Texture2D pixelTexture = null!;
        private SpriteFont textFont = null!;
        private bool isOpen;
        private Point mousePosition;

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

            mousePosition = currentMouseState.Position;

            if (UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState) &&
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
            Rectangle headerRectangle = new(panelRectangle.X, panelRectangle.Y, panelRectangle.Width, HeaderHeight);
            Rectangle refuelButtonRectangle = GetRefuelButtonRectangle(viewportWidth, viewportHeight);
            float missingFuel = world.Player.FuelTank.Capacity - world.Player.CurrentFuel;
            float affordableFuel = MathF.Min(missingFuel, (float)world.Player.Cash);
            bool canRefuel = affordableFuel > 0.0f;

            UiDrawHelper.DrawScreenDim(spriteBatch, pixelTexture, viewportWidth, viewportHeight);
            spriteBatch.Draw(pixelTexture, panelRectangle, new Color(22, 22, 22));
            spriteBatch.Draw(pixelTexture, headerRectangle, new Color(44, 44, 44));
            spriteBatch.Draw(pixelTexture, refuelButtonRectangle, canRefuel ? new Color(86, 110, 78) : new Color(64, 64, 64));
            bool isHovered = canRefuel && refuelButtonRectangle.Contains(mousePosition);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, refuelButtonRectangle, isHovered);

            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, panelRectangle, 2, new Color(108, 108, 108));
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, refuelButtonRectangle, 2, UiInteractionStyle.GetBorderColor(canRefuel ? new Color(152, 182, 140) : new Color(110, 110, 110), isHovered));

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Fuel Station", new Vector2(panelRectangle.X + ContentPadding, panelRectangle.Y + 12), new Color(244, 240, 229), TitleTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Fuel: {world.Player.CurrentFuel:0.00} / {world.Player.FuelTank.Capacity:0.00}", new Vector2(panelRectangle.X + ContentPadding, panelRectangle.Y + 72), new Color(230, 230, 230), BodyTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Can Buy: {affordableFuel:0.00}", new Vector2(panelRectangle.X + ContentPadding, panelRectangle.Y + 96), new Color(214, 214, 214), BodyTextScale);
            UiDrawHelper.DrawCenteredText(spriteBatch, textFont, "Refuel!", refuelButtonRectangle, new Color(248, 243, 233), ButtonTextScale);
        }

        private static float RefuelAllAffordable(ModelWorld world)
        {
            float missingFuel = world.Player.FuelTank.Capacity - world.Player.CurrentFuel;

            if (missingFuel <= 0.0f || world.Player.Cash <= 0.0)
            {
                return 0.0f;
            }

            float fuelPurchased = MathF.Min(missingFuel, (float)world.Player.Cash);
            world.Player.CurrentFuel += fuelPurchased;
            world.Player.Cash -= fuelPurchased;
            return fuelPurchased;
        }

        private Rectangle GetRefuelButtonRectangle(int viewportWidth, int viewportHeight)
        {
            int panelTop = (viewportHeight - PanelHeight) / 2;
            return new Rectangle((viewportWidth - ButtonWidth) / 2, panelTop + PanelHeight - ContentPadding - ButtonHeight + 6, ButtonWidth, ButtonHeight);
        }

        public bool IsPointerOverInteractiveElement(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            float missingFuel = world.Player.FuelTank.Capacity - world.Player.CurrentFuel;
            float affordableFuel = MathF.Min(missingFuel, (float)world.Player.Cash);
            return affordableFuel > 0.0f && GetRefuelButtonRectangle(viewportWidth, viewportHeight).Contains(mousePosition);
        }

        public void Close(ModelWorld world)
        {
            isOpen = false;
        }

    }
}
