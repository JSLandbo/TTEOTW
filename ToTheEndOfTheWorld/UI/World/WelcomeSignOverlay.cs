using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.UI.World
{
    public sealed class WelcomeSignOverlay : IInteractionOverlay
    {
        private const int ViewportPadding = 40;
        private Texture2D welcomeSignTexture = null!;
        private bool isOpen;

        public EBuildingInteraction Action => EBuildingInteraction.HouseInfoSign;
        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;
        public int PanelWidth => 0;

        public void Open(ABuilding building, int viewportWidth, int viewportHeight)
        {
            isOpen = true;
        }

        public void SetPanelOffset(int offsetX)
        {
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            welcomeSignTexture = content.Load<Texture2D>("General/WelcomeSign");
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, ModelWorld world, int viewportWidth, int viewportHeight)
        {
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            Rectangle destinationRectangle = GetDestinationRectangle(viewportWidth, viewportHeight);
            spriteBatch.Draw(welcomeSignTexture, destinationRectangle, Color.White);
        }

        public bool IsPointerOverInteractiveElement(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            return false;
        }

        public string? GetHoverLabel(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            return null;
        }

        public void Close(ModelWorld world)
        {
            isOpen = false;
        }

        private Rectangle GetDestinationRectangle(int viewportWidth, int viewportHeight)
        {
            float maxWidth = viewportWidth - (ViewportPadding * 2);
            float maxHeight = viewportHeight - (ViewportPadding * 2);
            float widthScale = maxWidth / welcomeSignTexture.Width;
            float heightScale = maxHeight / welcomeSignTexture.Height;
            float scale = MathF.Min(1f, MathF.Min(widthScale, heightScale));
            int width = (int)(welcomeSignTexture.Width * scale);
            int height = (int)(welcomeSignTexture.Height * scale);

            return new Rectangle(
                (viewportWidth - width) / 2,
                (viewportHeight - height) / 2,
                width,
                height);
        }
    }
}
