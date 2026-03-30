using ModelLibrary.Abstract.Types;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Items
{
    public sealed class Item : AType
    {
        public Item()
        {
        }

        public Item(Item other)
        {
            ID = other.ID;
            Name = other.Name;
            Worth = other.Worth;
            Weight = other.Weight;
            Stackable = other.Stackable;
        }

        [JsonConstructor]
        public Item(short ID, string Name, float Worth = 0.0f, float Weight = 0.0f, bool Stackable = true)
        {
            this.ID = ID;
            this.Name = Name;
            this.Worth = Worth;
            this.Weight = Weight;
            this.Stackable = Stackable;
        }
    }
}
