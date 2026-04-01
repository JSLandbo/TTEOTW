using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToTheEndOfTheWorld.Gameplay.Graphics
{
    public static class TextureAnimationHelper
    {
        private const double DefaultSecondsPerFrame = 0.1d;

        public static double TotalSeconds { get; set; }

        public static (Rectangle? SourceRectangle, int Width, int Height) GetFrame(int frames, Texture2D texture, double secondsPerFrame = DefaultSecondsPerFrame)
        {
            Rectangle? sourceRectangle = GetSourceRectangle(frames, texture, secondsPerFrame);

            return (sourceRectangle, sourceRectangle?.Width ?? texture.Width, sourceRectangle?.Height ?? texture.Height);
        }

        public static void Draw(SpriteBatch spriteBatch, Texture2D texture, Rectangle destinationRectangle, int frames, Color color, double secondsPerFrame = DefaultSecondsPerFrame)
        {
            spriteBatch.Draw(texture, destinationRectangle, GetSourceRectangle(frames, texture, secondsPerFrame), color);
        }

        public static Rectangle? GetSourceRectangle(int frames, Texture2D texture, double secondsPerFrame = DefaultSecondsPerFrame)
        {
            if (texture == null || texture.Width <= 0)
            {
                return null;
            }

            if (frames <= 1 || texture.Width % frames != 0)
            {
                return new Rectangle(0, 0, texture.Width, texture.Height);
            }

            int frameWidth = texture.Width / frames;
            int currentFrame = (int)(TotalSeconds / secondsPerFrame) % frames;

            return new Rectangle(currentFrame * frameWidth, 0, frameWidth, texture.Height);
        }
    }
}
