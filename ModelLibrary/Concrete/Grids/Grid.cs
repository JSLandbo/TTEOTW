using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;
using Newtonsoft.Json;

namespace ModelLibrary.Concrete.Grids
{
    public class Grid : AGrid
    {
        [JsonConstructor]
        public Grid(Vector2 InternalCoordinate, GridBox[,] InternalGrid)
        {
            this.InternalCoordinate = InternalCoordinate;
            this.InternalGrid = InternalGrid;

            for (int x = 0; x < InternalGrid.GetLength(0); x++)
            {
                for (int y = 0; y < InternalGrid.GetLength(1); y++)
                {
                    if (InternalGrid[x, y] is null)
                    {
                        InternalGrid[x, y] = new GridBox(null, 0);
                    }

                    InternalGrid[x, y].OwnerGrid = this;
                }
            }
        }
    }
}
