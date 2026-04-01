using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToTheEndOfTheWorld.UI.Common
{
    public static class UiDrawHelper
    {
        public static void DrawRectangleOutline(SpriteBatch spriteBatch, Texture2D pixelTexture, Rectangle rectangle, int thickness, Color color)
        {
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Bottom - thickness, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.Right - thickness, rectangle.Y, thickness, rectangle.Height), color);
        }
    }
}
