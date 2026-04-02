using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Common
{
    public static class UiDrawHelper
    {
        public static void DrawScreenDim(SpriteBatch spriteBatch, Texture2D pixelTexture, int viewportWidth, int viewportHeight)
        {
            spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, viewportWidth, viewportHeight), Color.Black * 0.45f);
        }

        public static void DrawRectangleOutline(SpriteBatch spriteBatch, Texture2D pixelTexture, Rectangle rectangle, int thickness, Color color)
        {
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Bottom - thickness, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.Right - thickness, rectangle.Y, thickness, rectangle.Height), color);
        }

        public static void DrawCenteredText(SpriteBatch spriteBatch, SpriteFont textFont, string text, Rectangle rectangle, Color color, float scale)
        {
            Vector2 size = textFont.MeasureString(text) * scale;
            Vector2 position = new(
                rectangle.X + ((rectangle.Width - size.X) / 2f),
                rectangle.Y + ((rectangle.Height - size.Y) / 2f));
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, text, position, color, scale);
        }
    }
}
