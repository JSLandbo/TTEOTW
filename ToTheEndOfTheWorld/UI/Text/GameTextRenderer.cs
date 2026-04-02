using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToTheEndOfTheWorld.UI.Text
{
    public static class GameTextRenderer
    {
        public static void DrawBoldString(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale = 1f)
        {
            Vector2 snappedPosition = new(
                (float)System.Math.Round(position.X),
                (float)System.Math.Round(position.Y));
            Color shadowColor = new((byte)0, (byte)0, (byte)0, (byte)(color.A * 0.8f));

            spriteBatch.DrawString(font, text, snappedPosition + new Vector2(1f, 1f), shadowColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(font, text, snappedPosition, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
