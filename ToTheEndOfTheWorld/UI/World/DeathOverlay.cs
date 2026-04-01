using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToTheEndOfTheWorld.UI.World
{
    public sealed class DeathOverlay(Texture2D youDiedTexture)
    {
        public void Draw(SpriteBatch spriteBatch, int viewportWidth, bool shouldShow)
        {
            if (!shouldShow)
            {
                return;
            }

            Rectangle destinationRectangle = new((viewportWidth - youDiedTexture.Width) / 2, 48, youDiedTexture.Width, youDiedTexture.Height);

            spriteBatch.Draw(youDiedTexture, destinationRectangle, Color.White);
        }
    }
}
