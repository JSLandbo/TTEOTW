using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ToTheEndOfTheWorld.Context
{
    public sealed class WorldEffectDefinitionsRepository : Dictionary<WorldEffectType, (Texture2D Texture, int SpriteFrames)>
    {
        public WorldEffectDefinitionsRepository(ContentManager content)
        {
            Add(WorldEffectType.Explosion, (content.Load<Texture2D>("General/ExplosionAnimation"), 4));
        }
    }
}
