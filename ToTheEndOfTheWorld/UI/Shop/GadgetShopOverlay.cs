using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class GadgetShopOverlay : IInteractionOverlay
    {
        private const int PanelWidth = 520;
        private const int PanelHeight = 240;
        private const float TitleTextScale = 1.35f;
        private const float BodyTextScale = 1.2f;

        private Texture2D pixelTexture = null!;
        private SpriteFont textFont = null!;
        private bool isOpen;

        public EBuildingInteraction Action => EBuildingInteraction.GadgetShop;
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
            }
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            Rectangle panelRectangle = new((viewportWidth - PanelWidth) / 2, (viewportHeight - PanelHeight) / 2, PanelWidth, PanelHeight);
            spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, viewportWidth, viewportHeight), Color.Black * 0.45f);
            spriteBatch.Draw(pixelTexture, panelRectangle, new Color(22, 22, 22));
            DrawRectangleOutline(spriteBatch, panelRectangle, 2, new Color(108, 108, 108));

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Gadget Store", new Vector2(panelRectangle.X + 20, panelRectangle.Y + 14), new Color(244, 240, 229), TitleTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Coming soon", new Vector2(panelRectangle.X + 22, panelRectangle.Y + 84), new Color(230, 230, 230), BodyTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Press E or Escape to close", new Vector2(panelRectangle.X + 20, panelRectangle.Bottom - 40), new Color(188, 188, 188), BodyTextScale);
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
