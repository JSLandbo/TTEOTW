using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Grids
{
    public class GridBox : AGridBox
    {
        [JsonConstructor]
        public GridBox(int ID, List<AType> Item)
        {
            this.ID = ID;
            this.Item = Item;
        }
    }
}