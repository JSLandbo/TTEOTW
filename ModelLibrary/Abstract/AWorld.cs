using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Buildings;

namespace ModelLibrary.Abstract
{
    public abstract class AWorld
    {
        public APlayer Player { get; set; } = null!;
        public List<ABuilding>? Buildings { get; set; } = [];
        public int BlocksWide { get; set; } = 0;
        public int BlocksHigh { get; set; } = 0;
        public Vector2 SpawnWorldPosition { get; set; } = Vector2.Zero;
        public Dictionary<Vector2, Vector2> WorldRender { get; set; } = [];
        public Dictionary<Vector2, bool> WorldTrails { get; set; } = [];
    }
}
