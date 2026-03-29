using ModelLibrary.Abstract.Types;

namespace ModelLibrary.Abstract.Grids
{
    public abstract class AGridBox
    {
        public int ID { get; set; }
        public List<AType> Item { get; set; }
    }
}