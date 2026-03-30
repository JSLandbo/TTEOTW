using ModelLibrary.Concrete.Blocks;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class BlockLootResolver
    {
        private readonly WorldElementsRepository blocks;

        public BlockLootResolver(WorldElementsRepository blocks)
        {
            this.blocks = blocks;
        }

        public bool TryResolve(short blockId, out Block loot, out int count)
        {
            loot = null;
            count = 0;

            if (!blocks.TryGetValue(blockId, out var blockDefinition) || blockId <= 0)
            {
                return false;
            }

            loot = new Block(blockDefinition.block)
            {
                Name = blockDefinition.Name
            };
            count = 1;
            return true;
        }
    }
}
