using Microsoft.Xna.Framework;

namespace ToTheEndOfTheWorld.UI.World
{
    public readonly struct WorldScreenTransform(int tileSize, int cameraOffsetX, int cameraOffsetY)
    {
        private readonly int tileSize = tileSize;
        private readonly int cameraOffsetX = cameraOffsetX;
        private readonly int cameraOffsetY = cameraOffsetY;
        private readonly int tileHalfSize = tileSize / 2;

        public Rectangle GetTileRectangle(Vector2 renderKey)
        {
            return GetTileRectangle((int)renderKey.X, (int)renderKey.Y);
        }

        public Rectangle GetTileRectangle(int renderKeyX, int renderKeyY, int tilesWide = 1, int tilesHigh = 1, int xOffset = 0, int yOffset = 0)
        {
            return new Rectangle(
                (renderKeyX * tileSize) - tileHalfSize - cameraOffsetX + xOffset,
                (renderKeyY * tileSize) - tileHalfSize - cameraOffsetY + yOffset,
                tilesWide * tileSize,
                tilesHigh * tileSize);
        }
    }
}
