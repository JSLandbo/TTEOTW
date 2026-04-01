using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.Gameplay.Graphics;

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

            APlayer player = world.Player;
            PlayerOrientation orientation = player.Orientation;
            bool drillExtended = player.DrillExtended;
            GameItemDefinition drill = items[player.Drill.ID];
            GameItemDefinition hull = items[player.Hull.ID];

            if (orientation.Equals(PlayerOrientation.Base))
            {
                DrawAnimatedTexture(spriteBatch, hull.Textures[PlayerOrientation.Base], hull.Frames, playerPosition);

                return;
            }

            if (drillExtended)
            {
                DrawAnimatedTexture(spriteBatch, hull.Textures[orientation], hull.Frames, playerPosition);

                float drillPositionX = playerPosition.X + (player.FacingDirection.X * tileSize);
                float drillPositionY = playerPosition.Y + (player.FacingDirection.Y * tileSize);

                DrawAnimatedTexture(spriteBatch, drill.Textures[orientation], drill.Frames, new Vector2(drillPositionX, drillPositionY));

                return;
            }

            DrawAnimatedTexture(spriteBatch, hull.Textures[PlayerOrientation.Base], hull.Frames, playerPosition);
        }

        private void DrawAnimatedTexture(SpriteBatch spriteBatch, Texture2D texture, int frames, Vector2 position)
        {
            Rectangle destinationRectangle = new((int)position.X, (int)position.Y, tileSize, tileSize);
            TextureAnimationHelper.Draw(spriteBatch, texture, destinationRectangle, frames, Color.White);
        }
    }
}
