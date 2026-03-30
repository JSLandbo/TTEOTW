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
            var definition = worldBlockDefinitionResolver.GetWorldBlock(x, y);
            var block = new Block(definition.Value.block);

            if (definition.Key == 2 && x > 0)
            {
                block.CurrentHealth += 0.01f * x;
                block.MaximumHealth += 0.01f * x;
            }

            return block;
        }
    }
}
