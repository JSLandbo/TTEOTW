using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ToTheEndOfTheWorld.Gameplay.Graphics;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Common
{
    public sealed class ItemSlotRenderer(ItemTextureResolver textureResolver, Texture2D pixelTexture, SpriteFont textFont, float stackTextScale = 1.05f)
    {
        private const float HoverItemScale = 0.95f;

        public void DrawGridSlot(SpriteBatch spriteBatch, Rectangle slotRectangle, AGridBox slot, Color backgroundColor, Color borderColor, bool showCount = true, bool isHovered = false)
        {
            spriteBatch.Draw(pixelTexture, slotRectangle, backgroundColor);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, slotRectangle, isHovered);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, slotRectangle, 2, UiInteractionStyle.GetBorderColor(borderColor, isHovered));

            if (slot.Item == null || slot.Count <= 0)
            {
                return;
            }

            DrawItem(spriteBatch, slot.Item, slotRectangle, isHovered ? HoverItemScale : 1.0f);

            if (showCount)
            {
                DrawStackCount(spriteBatch, slot.Count, slotRectangle);
            }
        }

        public void DrawItem(SpriteBatch spriteBatch, AType item, Rectangle bounds, float scale = 1.0f)
        {
            Texture2D texture = textureResolver.Resolve(item);

            int frames = textureResolver.ResolveFrames(item);
            var (SourceRectangle, Width, Height) = TextureAnimationHelper.GetFrame(frames, texture);
            spriteBatch.Draw(texture, GetNaturalTextureRectangle(bounds, Width, Height, scale), SourceRectangle, Color.White);
        }

        public void DrawItemFitted(SpriteBatch spriteBatch, AType item, Rectangle bounds, int padding = 3)
        {
            Texture2D texture = textureResolver.Resolve(item);

            int availableWidth = System.Math.Max(1, bounds.Width - (padding * 2));
            int availableHeight = System.Math.Max(1, bounds.Height - (padding * 2));
            float scaleX = availableWidth / (float)texture.Width;
            float scaleY = availableHeight / (float)texture.Height;
            float scale = System.MathF.Min(1.0f, System.MathF.Min(scaleX, scaleY));

            int frames = textureResolver.ResolveFrames(item);
            var (SourceRectangle, Width, Height) = TextureAnimationHelper.GetFrame(frames, texture);
            spriteBatch.Draw(texture, GetNaturalTextureRectangle(bounds, Width, Height, scale), SourceRectangle, Color.White);
        }

        public void DrawStackCount(SpriteBatch spriteBatch, int count, Rectangle bounds)
        {
            string countText = count.ToString();
            Vector2 countSize = textFont.MeasureString(countText) * stackTextScale;
            Vector2 countPosition = new(bounds.Right - countSize.X - 6, bounds.Bottom - countSize.Y - 4);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, countText, countPosition, Color.White, stackTextScale);
        }

        private static Rectangle GetNaturalTextureRectangle(Rectangle bounds, int textureWidth, int textureHeight, float scale)
        {
            int scaledWidth = System.Math.Max(1, (int)System.MathF.Round(textureWidth * scale));
            int scaledHeight = System.Math.Max(1, (int)System.MathF.Round(textureHeight * scale));

            return new Rectangle(
                bounds.X + ((bounds.Width - scaledWidth) / 2),
                bounds.Y + ((bounds.Height - scaledHeight) / 2),
                scaledWidth,
                scaledHeight);
        }

    }
}
