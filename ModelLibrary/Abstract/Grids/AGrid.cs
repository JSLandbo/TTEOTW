using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace ModelLibrary.Abstract.Grids
{
    public abstract class AGrid
    {
        public Vector2 InternalCoordinate { get; set; }
        public AGridBox[,] InternalGrid { get; set; }
        [JsonIgnore]
        public Action? OnChanged { get; set; }

        public void NotifyChanged()
        {
            OnChanged?.Invoke();
        }

        // TODO: Figure out what to do when grid size exceeds building window. Make Scrollable.
    }
}
