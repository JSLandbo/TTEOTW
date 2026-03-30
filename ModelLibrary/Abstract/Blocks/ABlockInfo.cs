using Microsoft.Xna.Framework;

namespace ModelLibrary.Abstract.Blocks
{
    public abstract class ABlockInfo
    {
        public long MinimumDepth { get; set; }
        public long MaximumDepth { get; set; }
        public Vector2 OccurrenceSpan { get; set; }
        public float Weight { get; set; }
    }
}
