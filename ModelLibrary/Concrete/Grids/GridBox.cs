using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Grids
{
    public class GridBox : AGridBox
    {
        [JsonConstructor]
        public GridBox(AType? Item, int Count)
        {
            this.Item = Item;
            this.Count = Count;
        }
    }
}
