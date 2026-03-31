using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Blocks;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Blocks
{
    public class BlockInfo : ABlockInfo
    {
        [JsonConstructor]
        public BlockInfo(long MinimumDepth = long.MinValue, long MaximumDepth = long.MaxValue, Vector2 OccurrenceSpan = new Vector2(), float Weight = 1.0f, float MiningHeatGeneration = 0.1f)
        {
            this.MinimumDepth = MinimumDepth;
            this.MaximumDepth = MaximumDepth;
            this.Weight = Weight;
            this.MiningHeatGeneration = MiningHeatGeneration;
            this.OccurrenceSpan = OccurrenceSpan;
        }
    }
}
