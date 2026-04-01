using System;
using Microsoft.Xna.Framework;

namespace ToTheEndOfTheWorld.UI.Common
{
    public static class UiColorHelper
    {
        public static Color Brighten(Color color, int amount)
        {
            return new Color( Math.Min(255, color.R + amount), Math.Min(255, color.G + amount), Math.Min(255, color.B + amount), color.A);
        }
    }
}
