using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Types;
using Newtonsoft.Json;

namespace ModelLibrary.Abstract.Grids
{
    public abstract class AGrid
    {
        public Vector2 InternalCoordinate { get; set; }
        public AGridBox[,] InternalGrid { get; set; } = null!;
        [JsonIgnore]
        public Action? OnChanged { get; set; }
        [JsonIgnore]
        public Func<AGridBox, AType, bool>? PlacementValidator { get; set; }

        public void NotifyChanged()
        {
            OnChanged?.Invoke();
        }

        public bool CanPlaceInSlot(AGridBox slot, AType item)
        {
            return PlacementValidator?.Invoke(slot, item) ?? true;
        }
    }
}
