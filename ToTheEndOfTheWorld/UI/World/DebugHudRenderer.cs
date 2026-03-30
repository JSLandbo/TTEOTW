using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.World
{
    public sealed class DebugHudRenderer
    {
        private const float DebugTextScale = 1.15f;
        private SpriteFont textFont;

        public void LoadContent(ContentManager content)
        {
            textFont = content.Load<SpriteFont>("Fonts/text");
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world)
        {
            var player = world.Player;
            var worldPosition = PlayerWorldPositionService.GetPlayerWorldPosition(world);
            var lineHeight = (int)(textFont.LineSpacing * DebugTextScale) + 2;
            var startX = 8;
            var startY = 8;

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"World Position: X: {worldPosition.X}, Y: {worldPosition.Y}", new Vector2(startX, startY), Color.Black, DebugTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Player Velocity: X: {player.XVelocity}, Y: {player.YVelocity}", new Vector2(startX, startY + lineHeight), Color.Black, DebugTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Player Offset: X: {player.XOffset}, Y: {player.YOffset}", new Vector2(startX, startY + (lineHeight * 2)), Color.Black, DebugTextScale);
        }
    }
}
