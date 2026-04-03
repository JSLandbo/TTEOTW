using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ToTheEndOfTheWorld.Context
{
    public sealed class ScreenEffectDefinitionsRepository : Dictionary<ScreenEffectType, (Texture2D Texture, int SpriteFrames, ScreenEffectDefinition Definition)>
    {
        public ScreenEffectDefinitionsRepository(ContentManager content)
        {
            Add(ScreenEffectType.Explosion,(content.Load<Texture2D>("General/ExplosionAnimation"), 4, new ScreenEffectDefinition(new Point(128, 128), 12)));
        }
    }
}
