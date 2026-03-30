using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Blocks;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Blocks
{
    public class BlockInfo : ABlockInfo
    {
        public BlockInfo()
        {

        }

        [JsonConstructor]
        public BlockInfo(long MinimumDepth = long.MinValue, long MaximumDepth = long.MaxValue, Vector2 OccurrenceSpan = new Vector2(), float Weight = 0.0f)
        {
            this.MinimumDepth = MinimumDepth;
            this.MaximumDepth = MaximumDepth;
            this.Weight = Weight;

            if (OccurrenceSpan is { })
            {
                this.OccurrenceSpan = OccurrenceSpan;
            }
            else
            {
                this.OccurrenceSpan = new Vector2(0, 0);
            }
        }
    }
}
