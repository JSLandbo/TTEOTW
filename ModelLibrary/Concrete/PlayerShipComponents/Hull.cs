using ModelLibrary.Abstract.PlayerShipComponents;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.PlayerShipComponents
{
    public class Hull : AHull
    {
        public Hull(Hull original)
        {
            ID = original.ID;
            Durability = original.Durability;
            Health = original.Health;
            Name = original.Name;
            Worth = original.Worth;
            Weight = original.Weight;
        }


        [JsonConstructor]
        public Hull(short ID, float Durability, float Health, string Name, float Worth, float Weight)
        {
            this.ID = ID;
            this.Durability = Durability;
            this.Health = Health;
            this.Name = Name;
            this.Worth = Worth;
            this.Weight = Weight;
        }
    }
}
