using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Buildings;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.World
{
    public sealed class WorldInteractionRenderer
    {
        private const float PromptTextScale = 1.3f;
        private readonly WorldBuildingTextureResolver textureResolver = new();
        private Texture2D pixelTexture;
        private SpriteFont textFont;

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White });
            textureResolver.LoadContent(content);
            textFont = content.Load<SpriteFont>("Fonts/text");
        }

        public void DrawBuildings(SpriteBatch spriteBatch, ModelWorld world, WorldViewportService worldViewportService, int tileSize)
        {
            if (world.Buildings == null)
            {
                return;
            }

            var centerWorldPosition = worldViewportService.GetCenterWorldPosition(world);
            var playerKey = world.Player.Coordinates;

            foreach (var building in world.Buildings)
            {
                if (!building.IsBackground)
                {
                    continue;
                }

                var renderKeyX = playerKey.X + (building.WorldX - centerWorldPosition.X);
                var renderKeyY = playerKey.Y + (building.WorldY - centerWorldPosition.Y);
                var location = new Vector2(
                    renderKeyX * tileSize - (0.5f * tileSize) - world.Player.XOffset + building.XOffset,
                    renderKeyY * tileSize - (0.5f * tileSize) - world.Player.YOffset + building.YOffset
                );

                var buildingRectangle = new Rectangle(
                    (int)location.X,
                    (int)location.Y,
                    building.TilesWide * tileSize,
                    building.TilesHigh * tileSize
                );

                var buildingTexture = textureResolver.Resolve(building);

                if (buildingTexture == null)
                {
                    spriteBatch.Draw(pixelTexture, buildingRectangle, Color.Black);
                    continue;
                }

                spriteBatch.Draw(buildingTexture, buildingRectangle, Color.White);
            }
        }

        public void DrawInteractionPrompt(SpriteBatch spriteBatch, ABuilding building, int viewportWidth, int viewportHeight)
        {
            if (building == null || string.IsNullOrWhiteSpace(building.InteractionPrompt))
            {
                return;
            }

            var promptSize = textFont.MeasureString(building.InteractionPrompt) * PromptTextScale;
            var promptRectangle = new Rectangle(
                (int)((viewportWidth - promptSize.X) / 2) - 16,
                viewportHeight - 64,
                (int)promptSize.X + 32,
                (int)promptSize.Y + 18
            );

            spriteBatch.Draw(pixelTexture, promptRectangle, new Color(0, 0, 0, 180));
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, building.InteractionPrompt, new Vector2(promptRectangle.X + 16, promptRectangle.Y + 8), Color.White, PromptTextScale);
        }
    }
}
