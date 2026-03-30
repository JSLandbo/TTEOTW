using ModelLibrary.Concrete.Blocks;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class WorldBlockFactory
    {
        private readonly WorldBlockDefinitionResolver worldBlockDefinitionResolver;

        public WorldBlockFactory(WorldBlockDefinitionResolver worldBlockDefinitionResolver)
        {
            this.worldBlockDefinitionResolver = worldBlockDefinitionResolver;
        }

        public Block CreateMutableWorldBlock(float x, float y)
        {
            System.Collections.Generic.KeyValuePair<int, (string Name, Microsoft.Xna.Framework.Graphics.Texture2D Texture, Block block)> definition = worldBlockDefinitionResolver.GetWorldBlock(x, y);
            Block block = new(definition.Value.block);

            if (definition.Key == 2 && x > 0)
            {
                block.CurrentHealth += 0.01f * x;
                block.MaximumHealth += 0.01f * x;
            }

            return block;
        }
    }
}
