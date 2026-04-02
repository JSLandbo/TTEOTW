using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.World
{
    public sealed class DebugHudRenderer
    {
        private const float DebugTextScale = 1.0f;
        private SpriteFont textFont;

        public void LoadContent(ContentManager content)
        {
            textFont = content.Load<SpriteFont>("File");
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world)
        {
            APlayer player = world.Player;
            Vector2 worldPosition = PlayerWorldPositionService.GetPlayerWorldPosition(world);
            int lineHeight = (int)(textFont.LineSpacing * DebugTextScale) + 2;
            int startX = 8;
            int startY = 8;

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"World Position: X: {worldPosition.X}, Y: {worldPosition.Y}", new Vector2(startX, startY), Color.Black, DebugTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Player Velocity: X: {player.XVelocity}, Y: {player.YVelocity}", new Vector2(startX, startY + lineHeight), Color.Black, DebugTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Player Offset: X: {player.XOffset}, Y: {player.YOffset}", new Vector2(startX, startY + (lineHeight * 2)), Color.Black, DebugTextScale);
        }
    }
}
