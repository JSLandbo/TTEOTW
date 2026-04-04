using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.Gameplay.Graphics;

namespace ToTheEndOfTheWorld.UI.World
{
    public sealed class PlayerShipRenderer(GameItemsRepository items, int tileSize)
    {
        private const int PropulsionFrames = 4;
        private readonly Dictionary<PlayerOrientation, Texture2D> propulsionTextures = [];

        public void LoadContent(ContentManager content)
        {
            propulsionTextures[PlayerOrientation.Left] = content.Load<Texture2D>("General/Propulsion/PropulsionLeft");
            propulsionTextures[PlayerOrientation.Right] = content.Load<Texture2D>("General/Propulsion/PropulsionRight");
            propulsionTextures[PlayerOrientation.Up] = content.Load<Texture2D>("General/Propulsion/PropulsionUp");
            propulsionTextures[PlayerOrientation.Down] = content.Load<Texture2D>("General/Propulsion/PropulsionDown");
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight, bool isGrounded)
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

            DrawPropulsion(spriteBatch, player, playerPosition, isGrounded);

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

        private void DrawPropulsion(SpriteBatch spriteBatch, APlayer player, Vector2 playerPosition, bool isGrounded)
        {
            if (!PlayerThrusterUsageService.UsesThrustersForMovement(player, isGrounded))
            {
                return;
            }

            if (player.MovementInput.X < 0)
            {
                DrawAnimatedTexture(spriteBatch, propulsionTextures[PlayerOrientation.Left], PropulsionFrames, new Vector2(playerPosition.X + tileSize, playerPosition.Y));
            }

            if (player.MovementInput.X > 0)
            {
                DrawAnimatedTexture(spriteBatch, propulsionTextures[PlayerOrientation.Right], PropulsionFrames, new Vector2(playerPosition.X - tileSize, playerPosition.Y));
            }

            if (player.MovementInput.Y < 0)
            {
                DrawAnimatedTexture(spriteBatch, propulsionTextures[PlayerOrientation.Up], PropulsionFrames, new Vector2(playerPosition.X, playerPosition.Y + tileSize));
            }

        }

        private void DrawAnimatedTexture(SpriteBatch spriteBatch, Texture2D texture, int frames, Vector2 position)
        {
            Rectangle destinationRectangle = new((int)position.X, (int)position.Y, tileSize, tileSize);
            TextureAnimationHelper.Draw(spriteBatch, texture, destinationRectangle, frames, Color.White);
        }
    }
}
