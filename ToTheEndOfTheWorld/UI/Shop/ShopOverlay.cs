using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete;
using ModelLibrary.Enums;
using System;
using ToTheEndOfTheWorld.Context;
using ToTheEndOfTheWorld.Gameplay;
using ToTheEndOfTheWorld.UI.Inventory;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class ShopOverlay : IInteractionOverlay
    {
        private const int PanelWidth = 860;
        private const int PanelHeight = 600;
        private const int HeaderHeight = 58;
        private const int CardHeight = 52;
        private const int ButtonWidth = 360;
        private const int ButtonHeight = 56;
        private const int SectionPadding = 22;
        private const int ValueListTop = 198;
        private const int ValueListHeight = 272;
        private const int ValueRowHeight = 46;
        private const int ValueIconSize = 34;
        private const float TitleTextScale = 1.35f;
        private const float BodyTextScale = 1.35f;
        private const float ListTextScale = 1.1f;
        private const float ButtonTextScale = 1.25f;
        private const float FooterTextScale = 1.1f;

        private readonly ShopService shopService;
        private readonly InventoryItemTextureResolver textureResolver;

        private Texture2D pixelTexture;
        private SpriteFont textFont;
        private bool isOpen;
        private int scrollOffset;

        public ShopOverlay(ShopService shopService, WorldElementsRepository blocks, GameItemsRepository items)
        {
            this.shopService = shopService;
            textureResolver = new InventoryItemTextureResolver(blocks, items);
        }

        public EBuildingInteraction Action => EBuildingInteraction.Shop;
        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;

        public void Open(ABuilding building)
        {
            isOpen = true;
            scrollOffset = 0;
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White });
            textFont = content.Load<SpriteFont>("Fonts/text");
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, World world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            if (WasJustPressed(currentKeyboardState, previousKeyboardState, Keys.Escape, Keys.E))
            {
                isOpen = false;
                return;
            }

            var listRectangle = GetValueListRectangle(viewportWidth, viewportHeight);
            var sellSummary = shopService.GetSellSummary(world);
            var entryCount = sellSummary.Entries.Count;
            var visibleRows = GetVisibleRowCount(listRectangle);
            var maxScrollOffset = Math.Max(0, entryCount - visibleRows);
            var scrollDelta = currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;

            if (scrollDelta != 0 && listRectangle.Contains(currentMouseState.Position))
            {
                scrollOffset -= Math.Sign(scrollDelta);
                scrollOffset = Math.Clamp(scrollOffset, 0, maxScrollOffset);
            }

            if (WasLeftClicked(currentMouseState, previousMouseState) && GetSellButtonRectangle(viewportWidth, viewportHeight).Contains(currentMouseState.Position))
            {
                shopService.SellAll(world);
            }
        }

        public void Draw(SpriteBatch spriteBatch, World world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            var panelRectangle = new Rectangle((viewportWidth - PanelWidth) / 2, (viewportHeight - PanelHeight) / 2, PanelWidth, PanelHeight);
            var headerRectangle = new Rectangle(panelRectangle.X, panelRectangle.Y, panelRectangle.Width, HeaderHeight);
            var cashCardRectangle = new Rectangle(panelRectangle.X + SectionPadding, panelRectangle.Y + 76, panelRectangle.Width - (SectionPadding * 2), CardHeight);
            var valueCardRectangle = new Rectangle(panelRectangle.X + SectionPadding, panelRectangle.Y + 136, panelRectangle.Width - (SectionPadding * 2), CardHeight);
            var valueListRectangle = GetValueListRectangle(viewportWidth, viewportHeight);
            var sellButtonRectangle = GetSellButtonRectangle(viewportWidth, viewportHeight);
            var sellSummary = shopService.GetSellSummary(world);
            var saleValue = Math.Floor(sellSummary.TotalValue);

            spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, viewportWidth, viewportHeight), Color.Black * 0.45f);
            spriteBatch.Draw(pixelTexture, new Rectangle(panelRectangle.X + 3, panelRectangle.Y + 4, panelRectangle.Width, panelRectangle.Height), new Color(0, 0, 0, 70));
            spriteBatch.Draw(pixelTexture, panelRectangle, new Color(22, 22, 22));
            spriteBatch.Draw(pixelTexture, headerRectangle, new Color(44, 44, 44));
            spriteBatch.Draw(pixelTexture, cashCardRectangle, new Color(34, 34, 34));
            spriteBatch.Draw(pixelTexture, valueCardRectangle, new Color(30, 30, 30));
            spriteBatch.Draw(pixelTexture, valueListRectangle, new Color(27, 27, 27));
            spriteBatch.Draw(pixelTexture, sellButtonRectangle, saleValue > 0 ? new Color(121, 106, 77) : new Color(64, 64, 64));

            DrawRectangleOutline(spriteBatch, panelRectangle, 2, new Color(108, 108, 108));
            DrawRectangleOutline(spriteBatch, cashCardRectangle, 1, new Color(78, 78, 78));
            DrawRectangleOutline(spriteBatch, valueCardRectangle, 1, new Color(78, 78, 78));
            DrawRectangleOutline(spriteBatch, valueListRectangle, 1, new Color(68, 68, 68));
            DrawRectangleOutline(spriteBatch, sellButtonRectangle, 2, saleValue > 0 ? new Color(181, 163, 126) : new Color(110, 110, 110));

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Shop", new Vector2(panelRectangle.X + 20, panelRectangle.Y + 12), new Color(244, 240, 229), TitleTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Money: {Math.Floor(world.Player.Cash)}", new Vector2(cashCardRectangle.X + 14, cashCardRectangle.Y + 10), new Color(232, 232, 232), BodyTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Sell Value: {saleValue}", new Vector2(valueCardRectangle.X + 14, valueCardRectangle.Y + 10), new Color(224, 224, 224), BodyTextScale);

            DrawSellableValueList(spriteBatch, sellSummary.Entries, valueListRectangle);
            DrawCenteredText(spriteBatch, saleValue > 0 ? $"Sell All ({saleValue})" : "Sell All", sellButtonRectangle, new Color(248, 243, 233), ButtonTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Press E or Escape to close", new Vector2(panelRectangle.X + 20, panelRectangle.Bottom - 40), new Color(188, 188, 188), FooterTextScale);
        }

        private void DrawSellableValueList(SpriteBatch spriteBatch, System.Collections.Generic.IReadOnlyList<ShopService.SellableInventoryEntry> sellableEntries, Rectangle rectangle)
        {
            var visibleRows = GetVisibleRowCount(rectangle);
            var maxScrollOffset = Math.Max(0, sellableEntries.Count - visibleRows);
            scrollOffset = Math.Clamp(scrollOffset, 0, maxScrollOffset);

            for (var visibleIndex = 0; visibleIndex < visibleRows; visibleIndex++)
            {
                var index = scrollOffset + visibleIndex;

                if (index >= sellableEntries.Count)
                {
                    break;
                }

                var entry = sellableEntries[index];
                var rowRectangle = new Rectangle(
                    rectangle.X + 10,
                    rectangle.Y + 10 + (visibleIndex * ValueRowHeight),
                    rectangle.Width - 20,
                    ValueRowHeight - 4);
                var iconRectangle = new Rectangle(rowRectangle.X + 6, rowRectangle.Y + 4, ValueIconSize, ValueIconSize);
                var textPosition = new Vector2(iconRectangle.Right + 12, rowRectangle.Y + 6);
                var texture = textureResolver.Resolve(entry.Item);

                spriteBatch.Draw(pixelTexture, rowRectangle, new Color(35, 35, 35));
                if (texture != null)
                {
                    spriteBatch.Draw(texture, iconRectangle, Color.White);
                }
                GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"{entry.Item.Name}  x{entry.Count}  |  {Math.Floor(entry.TotalValue)}", textPosition, new Color(236, 236, 236), ListTextScale);
            }

            if (sellableEntries.Count > visibleRows)
            {
                DrawScrollBar(spriteBatch, rectangle, sellableEntries.Count, visibleRows);
            }
        }

        private Rectangle GetSellButtonRectangle(int viewportWidth, int viewportHeight)
        {
            return new Rectangle((viewportWidth - ButtonWidth) / 2, (viewportHeight - PanelHeight) / 2 + 492, ButtonWidth, ButtonHeight);
        }

        private Rectangle GetValueListRectangle(int viewportWidth, int viewportHeight)
        {
            return new Rectangle((viewportWidth - PanelWidth) / 2 + SectionPadding, (viewportHeight - PanelHeight) / 2 + ValueListTop, PanelWidth - (SectionPadding * 2), ValueListHeight);
        }

        private int GetVisibleRowCount(Rectangle rectangle)
        {
            return Math.Max(1, (rectangle.Height - 20) / ValueRowHeight);
        }

        private void DrawScrollBar(SpriteBatch spriteBatch, Rectangle rectangle, int totalEntries, int visibleRows)
        {
            var trackRectangle = new Rectangle(rectangle.Right - 8, rectangle.Y + 10, 4, rectangle.Height - 20);
            var thumbHeight = Math.Max(24, (int)(trackRectangle.Height * (visibleRows / (float)totalEntries)));
            var maxScrollOffset = Math.Max(1, totalEntries - visibleRows);
            var thumbTravel = trackRectangle.Height - thumbHeight;
            var thumbY = trackRectangle.Y + (int)(thumbTravel * (scrollOffset / (float)maxScrollOffset));
            var thumbRectangle = new Rectangle(trackRectangle.X, thumbY, trackRectangle.Width, thumbHeight);

            spriteBatch.Draw(pixelTexture, trackRectangle, new Color(55, 55, 55));
            spriteBatch.Draw(pixelTexture, thumbRectangle, new Color(170, 170, 170));
        }

        private void DrawCenteredText(SpriteBatch spriteBatch, string text, Rectangle rectangle, Color color, float scale)
        {
            var size = textFont.MeasureString(text) * scale;
            var position = new Vector2(
                rectangle.X + ((rectangle.Width - size.X) / 2f),
                rectangle.Y + ((rectangle.Height - size.Y) / 2f));
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, text, position, color, scale);
        }

        private void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rectangle, int thickness, Color color)
        {
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Bottom - thickness, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.Right - thickness, rectangle.Y, thickness, rectangle.Height), color);
        }

        private static bool WasLeftClicked(MouseState currentState, MouseState previousState)
        {
            return currentState.LeftButton == ButtonState.Pressed && previousState.LeftButton == ButtonState.Released;
        }

        private static bool WasJustPressed(KeyboardState currentState, KeyboardState previousState, params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (currentState.IsKeyDown(key) && !previousState.IsKeyDown(key))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
