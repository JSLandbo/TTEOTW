using ModelLibrary.Abstract.Types;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Items
{
    public sealed class NukeDynamite : ADynamite
    {
        public NukeDynamite(NukeDynamite other)
        {
            ID = other.ID;
            Name = other.Name;
            Worth = other.Worth;
            Weight = other.Weight;
            Stackable = other.Stackable;
            ExplosionAreaSize = other.ExplosionAreaSize;
            ExplosionPlaybackFrames = other.ExplosionPlaybackFrames;
            Damage = other.Damage;
            MaxHardness = other.MaxHardness;
        }

        [JsonConstructor]
        public NukeDynamite(short ID, string Name, int ExplosionAreaSize, int ExplosionPlaybackFrames, float Damage, float MaxHardness, float Worth = 0.0f, float Weight = 0.0f, bool Stackable = true)
        {
            this.ID = ID;
            this.Name = Name;
            this.ExplosionAreaSize = ExplosionAreaSize;
            this.ExplosionPlaybackFrames = ExplosionPlaybackFrames;
            this.Damage = Damage;
            this.MaxHardness = MaxHardness;
            this.Worth = Worth;
            this.Weight = Weight;
            this.Stackable = Stackable;
        }
    }
}
