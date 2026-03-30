using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.World
{
    public sealed class DeathOverlay
    {
        private readonly SpriteFont textFont;

        public DeathOverlay(SpriteFont textFont)
        {
            this.textFont = textFont;
        }

        public void Draw(SpriteBatch spriteBatch, int viewportWidth, bool shouldShow)
        {
            if (!shouldShow)
            {
                return;
            }

            const string text = "You Died";
            const float scale = 2.0f;
            Vector2 size = textFont.MeasureString(text) * scale;
            Vector2 position = new((viewportWidth - size.X) / 2.0f, 84.0f);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, text, position, Color.White, scale);
        }
    }
}
