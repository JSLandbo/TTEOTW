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
            pixelTexture.SetData([Color.White]);
            textureResolver.LoadContent(content);
            textFont = content.Load<SpriteFont>("File");
        }

        public void DrawBuildings(SpriteBatch spriteBatch, ModelWorld world, WorldViewportService worldViewportService, WorldScreenTransform worldScreenTransform)
        {
            if (world.Buildings == null)
            {
                return;
            }

            Vector2 centerWorldPosition = worldViewportService.GetCenterWorldPosition(world);
            Vector2 playerKey = world.Player.Coordinates;

            foreach (ABuilding building in world.Buildings)
            {
                if (!building.IsBackground)
                {
                    continue;
                }

                int renderKeyX = (int)(playerKey.X + (building.WorldX - centerWorldPosition.X));
                int renderKeyY = (int)(playerKey.Y + (building.WorldY - centerWorldPosition.Y));

                Rectangle buildingRectangle = worldScreenTransform.GetTileRectangle(renderKeyX, renderKeyY, building.TilesWide, building.TilesHigh, building.XOffset, building.YOffset);

                Texture2D buildingTexture = textureResolver.Resolve(building);

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

            Vector2 promptSize = textFont.MeasureString(building.InteractionPrompt) * PromptTextScale;
            Rectangle promptRectangle = new(
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
