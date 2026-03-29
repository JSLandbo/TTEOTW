using ModelLibrary.Abstract.Types;

namespace ModelLibrary.Abstract.Grids
{
    public abstract class AGridBox
    {
        public AType? Item { get; set; }
        public int Count { get; set; }
    }
}
