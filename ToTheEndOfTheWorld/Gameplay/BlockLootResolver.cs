using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Concrete.Blocks;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class BlockLootResolver(WorldElementsRepository blocks)
    {
        public bool TryResolve(short blockId, out Block loot, out int count)
        {
            loot = null;
            count = 0;

            if (!blocks.TryGetValue(blockId, out (string Name, Texture2D Texture, int Frames, Block block) blockDefinition) || blockId <= 0)
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
