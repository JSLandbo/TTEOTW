using ModelLibrary.Abstract.Types;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Items
{
    public sealed class SmallDynamite : AConsumeable
    {
        public SmallDynamite(SmallDynamite other)
        {
            ID = other.ID;
            Name = other.Name;
            Worth = other.Worth;
            Weight = other.Weight;
            Stackable = other.Stackable;
            ExplosionAreaSize = other.ExplosionAreaSize;
            Damage = other.Damage;
            MaxHardness = other.MaxHardness;
        }

        [JsonConstructor]
        public SmallDynamite(short ID, string Name, int ExplosionAreaSize, float Damage, float MaxHardness, float Worth = 0.0f, float Weight = 0.0f, bool Stackable = true)
        {
            this.ID = ID;
            this.Name = Name;
            this.ExplosionAreaSize = ExplosionAreaSize;
            this.Damage = Damage;
            this.MaxHardness = MaxHardness;
            this.Worth = Worth;
            this.Weight = Weight;
            this.Stackable = Stackable;
        }

        public int ExplosionAreaSize { get; set; }
        public float Damage { get; set; }
        public float MaxHardness { get; set; }
    }
}
