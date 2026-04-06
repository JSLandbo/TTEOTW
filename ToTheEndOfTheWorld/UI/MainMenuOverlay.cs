using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI
{
    public sealed class MainMenuOverlay : IGameOverlay
    {
        private const int HeaderHeight = 58;
        private const int ButtonWidth = 200;
        private const int ButtonHeight = 48;
        private const int ButtonSpacing = 16;
        private const int ContentPadding = 24;
        private const float TitleTextScale = 1.15f;
        private const float ButtonTextScale = 1.0f;

        private static int PanelWidthValue => ButtonWidth + (ContentPadding * 2);
        private static int PanelHeight => HeaderHeight + ContentPadding + (ButtonHeight * 2) + ButtonSpacing + ContentPadding;

        private readonly Action onSave;
        private readonly Action onResetWorld;

        private Texture2D pixelTexture = null!;
        private SpriteFont textFont = null!;
        private bool isOpen;
        private Point mousePosition;

        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;

        public MainMenuOverlay(Action onSave, Action onResetWorld)
        {
            this.onSave = onSave;
            this.onResetWorld = onResetWorld;
        }

        public void Open()
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

            if (UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState))
            {
                if (GetSaveButtonRectangle(viewportWidth, viewportHeight).Contains(mousePosition))
                {
                    onSave?.Invoke();
                    isOpen = false;
                    return;
                }

                if (GetResetButtonRectangle(viewportWidth, viewportHeight).Contains(mousePosition))
                {
                    onResetWorld?.Invoke();
                    isOpen = false;
                    return;
                }
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
            Rectangle saveButtonRectangle = GetSaveButtonRectangle(viewportWidth, viewportHeight);
            Rectangle resetButtonRectangle = GetResetButtonRectangle(viewportWidth, viewportHeight);

            UiDrawHelper.DrawScreenDim(spriteBatch, pixelTexture, viewportWidth, viewportHeight);
            spriteBatch.Draw(pixelTexture, panelRectangle, UiColors.PanelBackground);
            spriteBatch.Draw(pixelTexture, headerRectangle, UiColors.HeaderBackground);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, panelRectangle, 2, UiColors.PanelBorder);

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Menu", new Vector2(panelRectangle.X + ContentPadding, panelRectangle.Y + 14), UiColors.TextTitle, TitleTextScale);

            DrawButton(spriteBatch, saveButtonRectangle, "Save", UiColors.ActionButtonBackgroundGreen, UiColors.ActionButtonBorderGreen);
            DrawButton(spriteBatch, resetButtonRectangle, "Reset World", UiColors.TrashButtonBackground, UiColors.TrashButtonBorder);
        }

        private void DrawButton(SpriteBatch spriteBatch, Rectangle buttonRectangle, string text, Color backgroundColor, Color borderColor)
        {
            bool isHovered = buttonRectangle.Contains(mousePosition);

            spriteBatch.Draw(pixelTexture, buttonRectangle, backgroundColor);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, buttonRectangle, isHovered);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, buttonRectangle, 2, UiInteractionStyle.GetBorderColor(borderColor, isHovered));
            UiDrawHelper.DrawCenteredText(spriteBatch, textFont, text, buttonRectangle, UiColors.TextButton, ButtonTextScale);
        }

        public bool IsPointerOverInteractiveElement(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            return GetSaveButtonRectangle(viewportWidth, viewportHeight).Contains(mousePosition)
                || GetResetButtonRectangle(viewportWidth, viewportHeight).Contains(mousePosition);
        }

        public void Close(ModelWorld world)
        {
            isOpen = false;
        }

        private Rectangle GetPanelRectangle(int viewportWidth, int viewportHeight)
        {
            return UiOverlayLayout.GetCenteredPanelRectangle(PanelWidthValue, PanelHeight, viewportWidth, viewportHeight);
        }

        private Rectangle GetSaveButtonRectangle(int viewportWidth, int viewportHeight)
        {
            Rectangle panelRectangle = GetPanelRectangle(viewportWidth, viewportHeight);
            int buttonX = panelRectangle.X + ((panelRectangle.Width - ButtonWidth) / 2);
            int buttonY = panelRectangle.Y + HeaderHeight + ContentPadding;
            return new Rectangle(buttonX, buttonY, ButtonWidth, ButtonHeight);
        }

        private Rectangle GetResetButtonRectangle(int viewportWidth, int viewportHeight)
        {
            Rectangle saveButton = GetSaveButtonRectangle(viewportWidth, viewportHeight);
            return new Rectangle(saveButton.X, saveButton.Bottom + ButtonSpacing, ButtonWidth, ButtonHeight);
        }
    }
}
