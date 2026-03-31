using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.UI.World
{
    public sealed class PlayerShipRenderer(GameItemsRepository items, int tileSize)
    {
        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            Vector2 playerPosition = new(
                (float)(viewportWidth / 2.0) - (0.5f * tileSize),
                (float)(viewportHeight / 2.0) - (0.5f * tileSize)
            );

            ModelLibrary.Abstract.APlayer player = world.Player;
            PlayerOrientation orientation = player.Orientation;
            bool drillExtended = player.DrillExtended;
            GameItemDefinition drill = items[player.Drill.ID];
            GameItemDefinition hull = items[player.Hull.ID];

            if (orientation.Equals(PlayerOrientation.Base))
            {
                spriteBatch.Draw(hull.Textures[PlayerOrientation.Base], playerPosition, Color.White);

                return;
            }

            if (drillExtended)
            {
                spriteBatch.Draw(hull.Textures[orientation], playerPosition, Color.White);

                float drillPositionX = playerPosition.X + (player.FacingDirection.X * tileSize);
                float drillPositionY = playerPosition.Y + (player.FacingDirection.Y * tileSize);

                spriteBatch.Draw(drill.Textures[orientation], new Vector2(drillPositionX, drillPositionY), Color.White);

                return;
            }

            spriteBatch.Draw(hull.Textures[PlayerOrientation.Base], playerPosition, Color.White);
        }
    }
}
