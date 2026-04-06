using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Context
{
    public sealed class ScreenEffectsRepository
    {
        private readonly List<ScreenEffect> effects = [];

        public IReadOnlyList<ScreenEffect> GetAll() => effects;

        public void Add(ScreenEffect effect)
        {
            effects.Add(effect);
        }

        public void Update()
        {
            for (int i = effects.Count - 1; i >= 0; i--)
            {
                if (!effects[i].AdvanceFrame())
                {
                    continue;
                }

                effects.RemoveAt(i);
            }
        }

        public void Clear()
        {
            effects.Clear();
        }
    }
}
