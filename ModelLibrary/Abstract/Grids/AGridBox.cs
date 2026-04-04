using ModelLibrary.Abstract.Types;
using Newtonsoft.Json;

namespace ModelLibrary.Abstract.Grids
{
    public abstract class AGridBox
    {
        private AType? item;
        private int count;

        [JsonIgnore]
        public AGrid? OwnerGrid { get; set; }

        public AType? Item
        {
            get => item;
            set
            {
                item = value;
                if (item == null) count = 0;
                OwnerGrid?.NotifyChanged();
            }
        }

        public int Count
        {
            get => count;
            set
            {
                count = Math.Max(0, value);
                if (count == 0) item = null;
                OwnerGrid?.NotifyChanged();
            }
        }
    }
}
