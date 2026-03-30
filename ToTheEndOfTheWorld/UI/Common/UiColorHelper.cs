using Microsoft.Xna.Framework;

namespace ToTheEndOfTheWorld.UI.Common
{
    public static class UiColorHelper
    {
        public static Color Brighten(Color color, int amount)
        {
            return new Color(
                System.Math.Min(255, color.R + amount),
                System.Math.Min(255, color.G + amount),
                System.Math.Min(255, color.B + amount),
                color.A);
        }
    }
}
