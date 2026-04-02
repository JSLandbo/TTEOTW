using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Context
{
    public sealed class WorldEffectsRepository
    {
        private readonly List<WorldEffect> effects = [];
        private readonly Dictionary<WorldTile, List<WorldEffect>> effectsByTile = [];

        public IReadOnlyList<WorldEffect> GetAll(WorldTile tile)
        {
            if (!effectsByTile.TryGetValue(tile, out List<WorldEffect> tileEffects))
            {
                return [];
            }

            return tileEffects;
        }

        public void Add(WorldEffect effect)
        {
            effects.Add(effect);

            if (!effectsByTile.TryGetValue(effect.Tile, out List<WorldEffect> tileEffects))
            {
                tileEffects = [];
                effectsByTile.Add(effect.Tile, tileEffects);
            }

            tileEffects.Add(effect);
        }

        public void Update()
        {
            for (int i = effects.Count - 1; i >= 0; i--)
            {
                WorldEffect effect = effects[i];

                if (!effect.AdvanceFrame())
                {
                    continue;
                }

                effects.RemoveAt(i);

                List<WorldEffect> tileEffects = effectsByTile[effect.Tile];
                tileEffects.Remove(effect);

                if (tileEffects.Count == 0)
                {
                    effectsByTile.Remove(effect.Tile);
                }
            }
        }
    }
}
