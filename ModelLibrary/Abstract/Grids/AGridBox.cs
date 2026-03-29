using ModelLibrary.Abstract.Types;
using Newtonsoft.Json;

namespace ModelLibrary.Abstract.Grids
{
    public abstract class AGridBox
    {
        private AType? item;
        private int count;

        [JsonIgnore]
        internal AGrid? OwnerGrid { get; set; }

        public AType? Item
        {
            get => item;
            set
            {
                item = value;
                OwnerGrid?.NotifyChanged();
            }
        }

        public int Count
        {
            get => count;
            set
            {
                count = value;
                OwnerGrid?.NotifyChanged();
            }
        }
    }
}
