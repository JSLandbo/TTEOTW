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
    public sealed class FuelStationOverlay(FuelStationService fuelStationService) : IInteractionOverlay
    {
        private const int HeaderHeight = 58;
        private const int ButtonWidth = 260;
        private const int ButtonHeight = 62;
        private const int ContentPadding = 20;
        private const int TextLineHeight = 24;
        private const int TextLines = 2;
        private const int TextTopMargin = 14;
        private const int ButtonTopMargin = 16;
        private const float TitleTextScale = 1.15f;
        private const float BodyTextScale = 1.0f;
        private const float ButtonTextScale = 1.0f;

        private static int PanelWidthValue => ButtonWidth + (ContentPadding * 2);
        private static int PanelHeight => HeaderHeight + TextTopMargin + (TextLines * TextLineHeight) + ButtonTopMargin + ButtonHeight + ContentPadding;

        private Texture2D pixelTexture = null!;
        private SpriteFont textFont = null!;
        private bool isOpen;
        private Point mousePosition;
        private int panelOffsetX;

        public EBuildingInteraction Action => EBuildingInteraction.FuelStation;
        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;
        public int PanelWidth => PanelWidthValue;

        public void Open(ABuilding building, int viewportWidth, int viewportHeight)
        {
            panelOffsetX = 0;
            isOpen = true;
        }

        public void SetPanelOffset(int offsetX) => panelOffsetX = offsetX;

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
                fuelStationService.TryRefuelAllAffordable(world);
            }
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            Rectangle panelRectangle = GetPanelRectangle(viewportWidth, viewportHeight);
            Rectangle headerRectangle = new(panelRectangle.X, panelRectangle.Y, panelRectangle.Width, HeaderHeight);
            Rectangle refuelButtonRectangle = GetRefuelButtonRectangle(viewportWidth, viewportHeight);
            float missingFuel = world.Player.FuelTank.Capacity - world.Player.CurrentFuel;
            float affordableFuel = MathF.Min(missingFuel, (float)(world.Player.Cash / fuelStationService.FuelPricePerUnit));
            bool canRefuel = affordableFuel > 0.0f;

            UiDrawHelper.DrawScreenDim(spriteBatch, pixelTexture, viewportWidth, viewportHeight);
            spriteBatch.Draw(pixelTexture, panelRectangle, UiColors.PanelBackground);
            spriteBatch.Draw(pixelTexture, headerRectangle, UiColors.HeaderBackground);
            spriteBatch.Draw(pixelTexture, refuelButtonRectangle, canRefuel ? UiColors.ActionButtonBackgroundGreen : UiColors.ButtonBackgroundDisabled);
            bool isHovered = canRefuel && refuelButtonRectangle.Contains(mousePosition);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, refuelButtonRectangle, isHovered);

            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, panelRectangle, 2, UiColors.PanelBorder);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, refuelButtonRectangle, 2, UiInteractionStyle.GetBorderColor(canRefuel ? UiColors.ActionButtonBorderGreen : UiColors.ButtonBorderDisabled, isHovered));

            int textY = panelRectangle.Y + HeaderHeight + TextTopMargin;
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Fuel Station", new Vector2(panelRectangle.X + ContentPadding, panelRectangle.Y + 12), UiColors.TextTitle, TitleTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Fuel: {world.Player.CurrentFuel:0.00} / {world.Player.FuelTank.Capacity:0.00}", new Vector2(panelRectangle.X + ContentPadding, textY), UiColors.TextBody, BodyTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Can Buy: {affordableFuel:0.00}", new Vector2(panelRectangle.X + ContentPadding, textY + TextLineHeight), UiColors.TextBodyAlt, BodyTextScale);
            UiDrawHelper.DrawCenteredText(spriteBatch, textFont, "Refuel!", refuelButtonRectangle, UiColors.TextButton, ButtonTextScale);
        }

        private Rectangle GetPanelRectangle(int viewportWidth, int viewportHeight)
        {
            return UiOverlayLayout.GetCenteredPanelRectangle(PanelWidthValue, PanelHeight, viewportWidth, viewportHeight, panelOffsetX);
        }

        private Rectangle GetRefuelButtonRectangle(int viewportWidth, int viewportHeight)
        {
            Rectangle panelRectangle = GetPanelRectangle(viewportWidth, viewportHeight);
            int buttonY = panelRectangle.Y + HeaderHeight + TextTopMargin + (TextLines * TextLineHeight) + ButtonTopMargin;
            return new Rectangle(panelRectangle.X + ((panelRectangle.Width - ButtonWidth) / 2), buttonY, ButtonWidth, ButtonHeight);
        }

        public bool IsPointerOverInteractiveElement(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            float missingFuel = world.Player.FuelTank.Capacity - world.Player.CurrentFuel;
            float affordableFuel = MathF.Min(missingFuel, (float)(world.Player.Cash / fuelStationService.FuelPricePerUnit));
            return affordableFuel > 0.0f && GetRefuelButtonRectangle(viewportWidth, viewportHeight).Contains(mousePosition);
        }

        public void Close(ModelWorld world)
        {
            isOpen = false;
        }
    }
}
