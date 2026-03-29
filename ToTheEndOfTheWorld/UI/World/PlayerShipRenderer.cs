using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Concrete;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.Context;

namespace ToTheEndOfTheWorld.UI.WorldRendering
{
    public sealed class PlayerShipRenderer
    {
        private readonly GameItemsRepository items;
        private readonly int tileSize;

        public PlayerShipRenderer(GameItemsRepository items, int tileSize)
        {
            this.items = items;
            this.tileSize = tileSize;
        }

        public void Draw(SpriteBatch spriteBatch, World world, int viewportWidth, int viewportHeight)
        {
            var playerPosition = new Vector2(
                (float)(viewportWidth / 2.0) - (0.5f * tileSize),
                (float)(viewportHeight / 2.0) - (0.5f * tileSize));

            var player = world.Player;
            var orientation = player.Orientation;
            var drillExtended = player.DrillExtended;
            var drill = items[player.Drill.ID];
            var hull = items[player.Hull.ID];

            if (orientation.Equals(PlayerOrientation.Base))
            {
                spriteBatch.Draw(hull.Textures[PlayerOrientation.Base], playerPosition, Color.White);
                return;
            }

            if (drillExtended)
            {
                spriteBatch.Draw(hull.Textures[orientation], playerPosition, Color.White);

                var drillPositionX = playerPosition.X + (player.FacingDirection.X * tileSize);
                var drillPositionY = playerPosition.Y + (player.FacingDirection.Y * tileSize);

                spriteBatch.Draw(drill.Textures[orientation], new Vector2(drillPositionX, drillPositionY), Color.White);
                return;
            }

            spriteBatch.Draw(hull.Textures[PlayerOrientation.Base], playerPosition, Color.White);
        }
    }
}
