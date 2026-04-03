using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Common
{
    public sealed class UiHoverLabelRenderer
    {
        private const float TextScale = 1.0f;
        private Texture2D pixelTexture = null!;
        private SpriteFont textFont = null!;

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData([Color.White]);
            textFont = content.Load<SpriteFont>("File");
        }

        public void Draw(SpriteBatch spriteBatch, string hoverLabel, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            if (string.IsNullOrWhiteSpace(hoverLabel))
            {
                return;
            }

            const int horizontalPadding = 14;
            const int verticalPadding = 8;
            const int cursorOffsetX = 18;
            const int cursorOffsetY = 22;
            const int screenPadding = 12;
            Vector2 textSize = textFont.MeasureString(hoverLabel) * TextScale;
            int width = (int)textSize.X + (horizontalPadding * 2);
            int height = (int)textSize.Y + (verticalPadding * 2);
            int x = mousePosition.X + cursorOffsetX;
            int y = mousePosition.Y + cursorOffsetY;

            if (x + width > viewportWidth - screenPadding)
            {
                x = viewportWidth - screenPadding - width;
            }

            if (y + height > viewportHeight - screenPadding)
            {
                y = mousePosition.Y - cursorOffsetY - height;
            }

            if (x < screenPadding)
            {
                x = screenPadding;
            }

            if (y < screenPadding)
            {
                y = screenPadding;
            }

            Rectangle labelRectangle = new(x, y, width, height);

            spriteBatch.Draw(pixelTexture, labelRectangle, new Color(24, 24, 24, 228));
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, labelRectangle, 1, new Color(102, 102, 102, 228));
            GameTextRenderer.DrawBoldString(
                spriteBatch,
                textFont,
                hoverLabel,
                new Vector2(labelRectangle.X + horizontalPadding, labelRectangle.Y + verticalPadding),
                new Color(244, 244, 244),
                TextScale);
        }
    }
}
