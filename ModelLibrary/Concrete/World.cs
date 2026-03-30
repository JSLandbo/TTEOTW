using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using ModelLibrary.Abstract.Buildings;

namespace ModelLibrary.Concrete
{
    public class World : AWorld
    {
        public World(Player Player, List<ABuilding> Buildings, int BlocksWide, int BlocksHigh, Dictionary<Vector2, Vector2> WorldRender, Dictionary<Vector2, bool> WorldTrails, Vector2? SpawnWorldPosition = null)
        {
            this.Player = Player;
            this.Buildings = Buildings;
            this.BlocksWide = BlocksWide;
            this.BlocksHigh = BlocksHigh;
            this.WorldRender = WorldRender;
            this.WorldTrails = WorldTrails;
            this.SpawnWorldPosition = SpawnWorldPosition ?? Vector2.Zero;
        }
    }
}
