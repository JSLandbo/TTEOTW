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

        public void Draw(SpriteBatch spriteBatch, string hoverLabel, int viewportWidth)
        {
            if (string.IsNullOrWhiteSpace(hoverLabel))
            {
                return;
            }

            const int horizontalPadding = 18;
            const int verticalPadding = 10;
            const int topMargin = 12;
            Vector2 textSize = textFont.MeasureString(hoverLabel) * TextScale;
            Rectangle labelRectangle = new(
                (viewportWidth / 2) - ((int)textSize.X / 2) - horizontalPadding,
                topMargin,
                (int)textSize.X + (horizontalPadding * 2),
                (int)textSize.Y + (verticalPadding * 2));

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
