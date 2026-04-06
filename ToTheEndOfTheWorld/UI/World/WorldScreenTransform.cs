using Microsoft.Xna.Framework;

namespace ToTheEndOfTheWorld.UI.World
{
    public readonly struct WorldScreenTransform(int tileSize, int cameraOffsetX, int cameraOffsetY, int windowWidth, int windowHeight, int blocksWide, int blocksHigh)
    {
        private readonly int tileSize = tileSize;
        private readonly int cameraOffsetX = cameraOffsetX;
        private readonly int cameraOffsetY = cameraOffsetY;
        
        // Player is always at pixel center of window
        // World offset aligns the player's grid position with window center
        private readonly int worldOffsetX = (windowWidth / 2) - ((int)(System.Math.Floor(blocksWide / 2.0) * tileSize) + (tileSize / 2));
        private readonly int worldOffsetY = (windowHeight / 2) - ((int)(System.Math.Floor(blocksHigh / 2.0) * tileSize) + (tileSize / 2));

        public Rectangle GetTileRectangle(Vector2 renderKey) => GetTileRectangle((int)renderKey.X, (int)renderKey.Y);

        public Rectangle GetTileRectangle(int renderKeyX, int renderKeyY, int tilesWide = 1, int tilesHigh = 1, int xOffset = 0, int yOffset = 0)
        {
            return new Rectangle(
                (renderKeyX * tileSize) - cameraOffsetX + xOffset + worldOffsetX,
                (renderKeyY * tileSize) - cameraOffsetY + yOffset + worldOffsetY,
                tilesWide * tileSize,
                tilesHigh * tileSize);
        }
    }
}
