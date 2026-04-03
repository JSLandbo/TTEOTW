using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ToTheEndOfTheWorld.Context;
using ToTheEndOfTheWorld.Gameplay.Graphics;

namespace ToTheEndOfTheWorld.UI.World
{
    public sealed class ScreenEffectRenderer
    {
        public void Draw(SpriteBatch spriteBatch, ScreenEffectsRepository screenEffects, ScreenEffectDefinitionsRepository definitions, int viewportWidth, int viewportHeight)
        {
            foreach (ScreenEffect effect in screenEffects.GetAll())
            {
                (Texture2D texture, int spriteFrames, ScreenEffectDefinition _) = definitions[effect.Type];
                Rectangle? sourceRectangle = TextureAnimationHelper.GetSourceRectangleForFrame(effect.PlayedFrames, spriteFrames, texture);
                Rectangle destinationRectangle = new(
                    (viewportWidth - effect.Size.X) / 2,
                    (viewportHeight - effect.Size.Y) / 2,
                    effect.Size.X,
                    effect.Size.Y);

                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
            }
        }
    }
}
