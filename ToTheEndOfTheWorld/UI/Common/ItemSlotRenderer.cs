using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Common
{
    public sealed class ItemSlotRenderer
    {
        private const float HoverItemScale = 0.95f;
        private readonly ItemTextureResolver textureResolver;
        private readonly Texture2D pixelTexture;
        private readonly SpriteFont textFont;
        private readonly float stackTextScale;

        public ItemSlotRenderer(ItemTextureResolver textureResolver, Texture2D pixelTexture, SpriteFont textFont, float stackTextScale = 1.05f)
        {
            this.textureResolver = textureResolver;
            this.pixelTexture = pixelTexture;
            this.textFont = textFont;
            this.stackTextScale = stackTextScale;
        }

        public void DrawGridSlot(SpriteBatch spriteBatch, Rectangle slotRectangle, AGridBox slot, Color backgroundColor, Color borderColor, bool showCount = true, bool isHovered = false)
        {
            spriteBatch.Draw(pixelTexture, slotRectangle, backgroundColor);
            if (isHovered)
            {
                spriteBatch.Draw(pixelTexture, slotRectangle, Color.White * 0.08f);
            }

            DrawRectangleOutline(spriteBatch, slotRectangle, 2, isHovered ? UiColorHelper.Brighten(borderColor, 36) : borderColor);

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
            var texture = textureResolver.Resolve(item);

            if (texture == null)
            {
                var fallbackRectangle = ScaleRectangleInside(bounds, scale, 3);
                spriteBatch.Draw(pixelTexture, fallbackRectangle, new Color(118, 87, 53));

                var itemName = string.IsNullOrWhiteSpace(item.Name) ? $"ID{item.ID}" : item.Name;
                var label = itemName.Length > 3 ? itemName[..3].ToUpperInvariant() : itemName.ToUpperInvariant();
                GameTextRenderer.DrawBoldString(spriteBatch, textFont, label, new Vector2(bounds.X + 4, bounds.Y + 2), Color.White, stackTextScale);
                return;
            }

            spriteBatch.Draw(texture, GetNaturalTextureRectangle(bounds, texture.Width, texture.Height, scale), Color.White);
        }

        public void DrawItemFitted(SpriteBatch spriteBatch, AType item, Rectangle bounds, int padding = 3)
        {
            var texture = textureResolver.Resolve(item);

            if (texture == null)
            {
                DrawItem(spriteBatch, item, bounds);
                return;
            }

            var availableWidth = System.Math.Max(1, bounds.Width - (padding * 2));
            var availableHeight = System.Math.Max(1, bounds.Height - (padding * 2));
            var scaleX = availableWidth / (float)texture.Width;
            var scaleY = availableHeight / (float)texture.Height;
            var scale = System.MathF.Min(1.0f, System.MathF.Min(scaleX, scaleY));

            spriteBatch.Draw(texture, GetNaturalTextureRectangle(bounds, texture.Width, texture.Height, scale), Color.White);
        }

        public void DrawStackCount(SpriteBatch spriteBatch, int count, Rectangle bounds)
        {
            var countText = count.ToString();
            var countSize = textFont.MeasureString(countText) * stackTextScale;
            var countPosition = new Vector2(bounds.Right - countSize.X - 6, bounds.Bottom - countSize.Y - 4);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, countText, countPosition, Color.White, stackTextScale);
        }

        private void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rectangle, int thickness, Color color)
        {
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Bottom - thickness, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.Right - thickness, rectangle.Y, thickness, rectangle.Height), color);
        }

        private static Rectangle GetNaturalTextureRectangle(Rectangle bounds, int textureWidth, int textureHeight, float scale)
        {
            var scaledWidth = System.Math.Max(1, (int)System.MathF.Round(textureWidth * scale));
            var scaledHeight = System.Math.Max(1, (int)System.MathF.Round(textureHeight * scale));

            return new Rectangle(
                bounds.X + ((bounds.Width - scaledWidth) / 2),
                bounds.Y + ((bounds.Height - scaledHeight) / 2),
                scaledWidth,
                scaledHeight);
        }

        private static Rectangle ScaleRectangleInside(Rectangle bounds, float scale, int inset)
        {
            var innerRectangle = new Rectangle(bounds.X + inset, bounds.Y + inset, bounds.Width - (inset * 2), bounds.Height - (inset * 2));
            var scaledWidth = System.Math.Max(1, (int)System.MathF.Round(innerRectangle.Width * scale));
            var scaledHeight = System.Math.Max(1, (int)System.MathF.Round(innerRectangle.Height * scale));

            return new Rectangle(
                innerRectangle.X + ((innerRectangle.Width - scaledWidth) / 2),
                innerRectangle.Y + ((innerRectangle.Height - scaledHeight) / 2),
                scaledWidth,
                scaledHeight);
        }
    }
}
