using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToTheEndOfTheWorld.UI.Common
{
    public static class UiInteractionStyle
    {
        private const float HoverOverlayOpacity = 0.08f;
        private const int HoverBorderBrightenAmount = 28;

        public static void DrawHoverOverlay(SpriteBatch spriteBatch, Texture2D pixelTexture, Rectangle rectangle, bool isHovered)
        {
            if (!isHovered)
            {
                return;
            }

            spriteBatch.Draw(pixelTexture, rectangle, Color.White * HoverOverlayOpacity);
        }

        public static Color GetBorderColor(Color baseColor, bool isHovered)
        {
            return isHovered ? UiColorHelper.Brighten(baseColor, HoverBorderBrightenAmount) : baseColor;
        }
    }
}
