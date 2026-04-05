using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class ShopOverlay(ShopService shopService, WorldElementsRepository blocks, GameItemsRepository items) : IInteractionOverlay
    {
        private const int HeaderHeight = 58;
        private const int CardHeight = 52;
        private const int SmallButtonWidth = 180;
        private const int SmallButtonHeight = 72;
        private const int SmallButtonGap = 16;
        private const int SellSpecificWidth = 420;
        private const int SellSpecificHeight = 72;
        private const int SectionPadding = 20;
        private const int CardTopMargin = 18;
        private const int ListTopMargin = 10;
        private const int ValueListHeight = 324;
        private const int ValueRowHeight = 76;
        private const int ValueColumnGap = 12;
        private const int ValueEntryPadding = 10;
        private const int ValueIconSize = 64;
        private const int ButtonsBottomMargin = 20;
        private const int ButtonsTopMargin = 16;
        private const float TitleTextScale = 1.15f;
        private const float BodyTextScale = 1.05f;
        private const float ListTitleTextScale = 1.0f;
        private const float ListBodyTextScale = 0.95f;
        private const float SmallButtonTextScale = 0.95f;

        private static int PanelWidthValue => (SectionPadding * 2) + (SmallButtonWidth * 2) + SmallButtonGap + SellSpecificWidth + SmallButtonGap;
        private static int PanelHeight => HeaderHeight + CardTopMargin + CardHeight + ListTopMargin + ValueListHeight + ButtonsTopMargin + SmallButtonHeight + ButtonsBottomMargin;

        private readonly ItemTextureResolver textureResolver = new(blocks, items);

        private ItemSlotRenderer slotRenderer;
        private Texture2D pixelTexture;
        private SpriteFont textFont;
        private ABuilding currentBuilding;
        private bool isOpen;
        private int scrollOffset;
        private Point mousePosition;
        private bool isSellModeActive;
        private int panelOffsetX;

        public EBuildingInteraction Action => EBuildingInteraction.Shop;
        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;
        public int PanelWidth => PanelWidthValue;

        public bool TrySellSlot(ModelWorld world, AGridBox slot)
        {
            if (!isOpen || !isSellModeActive || slot == null)
            {
                return false;
            }

            return shopService.SellSlot(world, slot) > 0;
        }

        public void Open(ABuilding building, int viewportWidth, int viewportHeight)
        {
            currentBuilding = building;
            panelOffsetX = 0;
            isOpen = true;
            scrollOffset = 0;
        }

        public void SetPanelOffset(int offsetX) => panelOffsetX = offsetX;

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData([Color.White]);
            textFont = content.Load<SpriteFont>("File");
            slotRenderer = new ItemSlotRenderer(textureResolver, pixelTexture, textFont);
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            Rectangle listRectangle = GetValueListRectangle(viewportWidth, viewportHeight);
            ShopService.SellSummary sellSummary = shopService.GetSellSummary(world);
            int totalRows = GetTotalValueRows(sellSummary.Entries.Count);
            int visibleRows = GetVisibleRowCount(listRectangle);
            int maxScrollOffset = Math.Max(0, totalRows - visibleRows);
            int scrollDelta = currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;
            mousePosition = currentMouseState.Position;
            isSellModeActive = currentKeyboardState.IsKeyDown(Keys.LeftControl) || currentKeyboardState.IsKeyDown(Keys.RightControl);

            if (scrollDelta != 0 && listRectangle.Contains(currentMouseState.Position))
            {
                scrollOffset -= Math.Sign(scrollDelta);
                scrollOffset = Math.Clamp(scrollOffset, 0, maxScrollOffset);
            }

            if (!UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState))
            {
                return;
            }

            if (GetSellAllButtonRectangle(viewportWidth, viewportHeight).Contains(currentMouseState.Position))
            {
                shopService.SellAll(world);

                return;
            }

            if (GetSellOresButtonRectangle(viewportWidth, viewportHeight).Contains(currentMouseState.Position))
            {
                shopService.SellOres(world);
            }
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            Rectangle panelRectangle = GetPanelRectangle(viewportWidth, viewportHeight);
            Rectangle headerRectangle = new(panelRectangle.X, panelRectangle.Y, panelRectangle.Width, HeaderHeight);
            Rectangle valueCardRectangle = new(panelRectangle.X + SectionPadding, panelRectangle.Y + HeaderHeight + CardTopMargin, panelRectangle.Width - (SectionPadding * 2), CardHeight);
            Rectangle valueListRectangle = GetValueListRectangle(viewportWidth, viewportHeight);
            Rectangle sellAllButtonRectangle = GetSellAllButtonRectangle(viewportWidth, viewportHeight);
            Rectangle sellOresButtonRectangle = GetSellOresButtonRectangle(viewportWidth, viewportHeight);
            Rectangle sellSpecificRectangle = GetSellSpecificRectangle(viewportWidth, viewportHeight);
            ShopService.SellSummary sellSummary = shopService.GetSellSummary(world);
            ShopService.SellSummary oreSellSummary = shopService.GetOreSellSummary(world);
            double saleValue = sellSummary.TotalValue;
            double oreSaleValue = oreSellSummary.TotalValue;

            if (currentBuilding?.ShowPlayerInventoryWhenOpen != true)
            {
                UiDrawHelper.DrawScreenDim(spriteBatch, pixelTexture, viewportWidth, viewportHeight);
            }

            spriteBatch.Draw(pixelTexture, new Rectangle(panelRectangle.X + 3, panelRectangle.Y + 4, panelRectangle.Width, panelRectangle.Height), UiColors.PanelShadow);
            spriteBatch.Draw(pixelTexture, panelRectangle, UiColors.PanelBackground);
            spriteBatch.Draw(pixelTexture, headerRectangle, UiColors.HeaderBackground);
            spriteBatch.Draw(pixelTexture, valueCardRectangle, UiColors.CardBackground);
            spriteBatch.Draw(pixelTexture, valueListRectangle, UiColors.ListBackground);

            // Sell All button
            spriteBatch.Draw(pixelTexture, sellAllButtonRectangle, saleValue > 0 ? UiColors.ActionButtonBackground : UiColors.ButtonBackgroundDisabled);
            bool isSellAllHovered = saleValue > 0 && sellAllButtonRectangle.Contains(mousePosition);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, sellAllButtonRectangle, isSellAllHovered);

            // Sell Ores button
            spriteBatch.Draw(pixelTexture, sellOresButtonRectangle, oreSaleValue > 0 ? UiColors.ActionButtonBackground : UiColors.ButtonBackgroundDisabled);
            bool isSellOresHovered = oreSaleValue > 0 && sellOresButtonRectangle.Contains(mousePosition);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, sellOresButtonRectangle, isSellOresHovered);

            // Sell Specific hint area
            spriteBatch.Draw(pixelTexture, sellSpecificRectangle, isSellModeActive ? UiColors.ActionButtonBackgroundGreenAlt : UiColors.SectionBackgroundDark);

            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, panelRectangle, 2, UiColors.PanelBorder);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, valueCardRectangle, 1, UiColors.CardBorder);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, valueListRectangle, 1, UiColors.ListBorder);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, sellAllButtonRectangle, 2, UiInteractionStyle.GetBorderColor(saleValue > 0 ? UiColors.ActionButtonBorder : UiColors.ButtonBorderDisabled, isSellAllHovered));
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, sellOresButtonRectangle, 2, UiInteractionStyle.GetBorderColor(oreSaleValue > 0 ? UiColors.ActionButtonBorder : UiColors.ButtonBorderDisabled, isSellOresHovered));
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, sellSpecificRectangle, 2, isSellModeActive ? UiColors.ActionButtonBorderGreenAlt : UiColors.PanelBorderDark);

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Shop", new Vector2(panelRectangle.X + 20, panelRectangle.Y + 12), UiColors.TextTitle, TitleTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Sell Value: {saleValue:0.##}", new Vector2(valueCardRectangle.X + 14, valueCardRectangle.Y + 10), UiColors.TextBodyAlt2, BodyTextScale);

            DrawSellableValueList(spriteBatch, sellSummary.Entries, valueListRectangle);

            // Button text - two lines
            DrawTwoLineButton(spriteBatch, sellAllButtonRectangle, "Sell all", saleValue > 0 ? $"{saleValue:0.##}" : "");
            DrawTwoLineButton(spriteBatch, sellOresButtonRectangle, "Sell ores", oreSaleValue > 0 ? $"{oreSaleValue:0.##}" : "");

            // Sell specific hint - three lines centered
            DrawSellSpecificHint(spriteBatch, sellSpecificRectangle);
        }

        private void DrawTwoLineButton(SpriteBatch spriteBatch, Rectangle buttonRect, string topText, string bottomText)
        {
            float topY = buttonRect.Y + 14;
            float bottomY = buttonRect.Y + 42;
            Vector2 topSize = textFont.MeasureString(topText) * SmallButtonTextScale;
            Vector2 bottomSize = textFont.MeasureString(bottomText) * SmallButtonTextScale;
            float topX = buttonRect.X + ((buttonRect.Width - topSize.X) / 2);
            float bottomX = buttonRect.X + ((buttonRect.Width - bottomSize.X) / 2);

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, topText, new Vector2(topX, topY), UiColors.TextButton, SmallButtonTextScale);
            if (!string.IsNullOrEmpty(bottomText))
            {
                GameTextRenderer.DrawBoldString(spriteBatch, textFont, bottomText, new Vector2(bottomX, bottomY), UiColors.TextButtonAlt, SmallButtonTextScale);
            }
        }

        private void DrawSellSpecificHint(SpriteBatch spriteBatch, Rectangle rect)
        {
            Color textColor = isSellModeActive ? UiColors.TextButton : UiColors.TextMuted;
            string line1 = "Or..";
            string line2 = "CTRL + click items in your inventory";
            string line3 = "to sell them!";

            Vector2 size1 = textFont.MeasureString(line1) * SmallButtonTextScale;
            Vector2 size2 = textFont.MeasureString(line2) * SmallButtonTextScale;
            Vector2 size3 = textFont.MeasureString(line3) * SmallButtonTextScale;

            float y1 = rect.Y + 6;
            float y2 = rect.Y + 24;
            float y3 = rect.Y + 42;

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, line1, new Vector2(rect.X + ((rect.Width - size1.X) / 2), y1), textColor, SmallButtonTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, line2, new Vector2(rect.X + ((rect.Width - size2.X) / 2), y2), textColor, SmallButtonTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, line3, new Vector2(rect.X + ((rect.Width - size3.X) / 2), y3), textColor, SmallButtonTextScale);
        }

        private void DrawSellableValueList(SpriteBatch spriteBatch, System.Collections.Generic.IReadOnlyList<ShopService.SellableInventoryEntry> sellableEntries, Rectangle rectangle)
        {
            int visibleRows = GetVisibleRowCount(rectangle);
            int totalRows = GetTotalValueRows(sellableEntries.Count);
            int maxScrollOffset = Math.Max(0, totalRows - visibleRows);
            scrollOffset = Math.Clamp(scrollOffset, 0, maxScrollOffset);

            for (int visibleIndex = 0; visibleIndex < visibleRows; visibleIndex++)
            {
                int rowIndex = scrollOffset + visibleIndex;
                int firstEntryIndex = rowIndex * 2;

                if (firstEntryIndex >= sellableEntries.Count)
                {
                    break;
                }

                Rectangle firstEntryRectangle = GetValueEntryRectangle(rectangle, visibleIndex, 0);
                DrawSellableEntry(spriteBatch, sellableEntries[firstEntryIndex], firstEntryRectangle);

                int secondEntryIndex = firstEntryIndex + 1;
                if (secondEntryIndex < sellableEntries.Count)
                {
                    Rectangle secondEntryRectangle = GetValueEntryRectangle(rectangle, visibleIndex, 1);
                    DrawSellableEntry(spriteBatch, sellableEntries[secondEntryIndex], secondEntryRectangle);
                }
            }

            if (totalRows > visibleRows)
            {
                DrawScrollBar(spriteBatch, rectangle, totalRows, visibleRows);
            }
        }

        private void DrawSellableEntry(SpriteBatch spriteBatch, ShopService.SellableInventoryEntry entry, Rectangle entryRectangle)
        {
            Rectangle iconRectangle = new(
                entryRectangle.X + ValueEntryPadding,
                entryRectangle.Y + ((entryRectangle.Height - ValueIconSize) / 2),
                ValueIconSize,
                ValueIconSize);
            Vector2 titlePosition = new(iconRectangle.Right + 14, entryRectangle.Y + 8);
            Vector2 detailPosition = new(iconRectangle.Right + 14, entryRectangle.Y + 38);
            spriteBatch.Draw(pixelTexture, entryRectangle, UiColors.ListEntryBackground);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, entryRectangle, 1, UiColors.ListEntryBorder);

            slotRenderer.DrawItem(spriteBatch, entry.Item, iconRectangle);

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, entry.Item.Name, titlePosition, UiColors.TextListTitle, ListTitleTextScale);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"x{entry.Count}  |  {entry.TotalValue:0.##}", detailPosition, UiColors.TextListBody, ListBodyTextScale);
        }

        private Rectangle GetValueEntryRectangle(Rectangle listRectangle, int visibleRowIndex, int columnIndex)
        {
            int availableWidth = listRectangle.Width - 20 - 8 - ValueColumnGap;
            int entryWidth = availableWidth / 2;
            int entryX = listRectangle.X + 10 + (columnIndex * (entryWidth + ValueColumnGap));
            int entryY = listRectangle.Y + 10 + (visibleRowIndex * ValueRowHeight);

            return new Rectangle(entryX, entryY, entryWidth, ValueRowHeight - 6);
        }

        private Rectangle GetSellAllButtonRectangle(int viewportWidth, int viewportHeight)
        {
            Rectangle panelRectangle = GetPanelRectangle(viewportWidth, viewportHeight);
            int buttonsTop = panelRectangle.Bottom - ButtonsBottomMargin - SmallButtonHeight;
            int buttonsLeft = panelRectangle.X + SectionPadding;

            return new Rectangle(buttonsLeft, buttonsTop, SmallButtonWidth, SmallButtonHeight);
        }

        private Rectangle GetSellOresButtonRectangle(int viewportWidth, int viewportHeight)
        {
            Rectangle sellAllButtonRectangle = GetSellAllButtonRectangle(viewportWidth, viewportHeight);
            return new Rectangle(sellAllButtonRectangle.Right + SmallButtonGap, sellAllButtonRectangle.Y, SmallButtonWidth, SmallButtonHeight);
        }

        private Rectangle GetSellSpecificRectangle(int viewportWidth, int viewportHeight)
        {
            Rectangle panelRectangle = GetPanelRectangle(viewportWidth, viewportHeight);
            int buttonsTop = panelRectangle.Bottom - ButtonsBottomMargin - SmallButtonHeight;
            int rightEdge = panelRectangle.Right - SectionPadding;

            return new Rectangle(rightEdge - SellSpecificWidth, buttonsTop, SellSpecificWidth, SellSpecificHeight);
        }

        private Rectangle GetValueListRectangle(int viewportWidth, int viewportHeight)
        {
            Rectangle panelRectangle = GetPanelRectangle(viewportWidth, viewportHeight);
            int listTop = panelRectangle.Y + HeaderHeight + CardTopMargin + CardHeight + ListTopMargin;
            return new Rectangle(panelRectangle.X + SectionPadding, listTop, panelRectangle.Width - (SectionPadding * 2), ValueListHeight);
        }

        private int GetVisibleRowCount(Rectangle rectangle)
        {
            return Math.Max(1, (rectangle.Height - 20) / ValueRowHeight);
        }

        private int GetTotalValueRows(int entryCount)
        {
            return Math.Max(0, (entryCount + 1) / 2);
        }

        private void DrawScrollBar(SpriteBatch spriteBatch, Rectangle rectangle, int totalEntries, int visibleRows)
        {
            Rectangle trackRectangle = new(rectangle.Right - 8, rectangle.Y + 10, 4, rectangle.Height - 20);
            int thumbHeight = Math.Max(24, (int)(trackRectangle.Height * (visibleRows / (float)totalEntries)));
            int maxScrollOffset = Math.Max(1, totalEntries - visibleRows);
            int thumbTravel = trackRectangle.Height - thumbHeight;
            int thumbY = trackRectangle.Y + (int)(thumbTravel * (scrollOffset / (float)maxScrollOffset));
            Rectangle thumbRectangle = new(trackRectangle.X, thumbY, trackRectangle.Width, thumbHeight);

            spriteBatch.Draw(pixelTexture, trackRectangle, UiColors.ScrollbarTrack);
            spriteBatch.Draw(pixelTexture, thumbRectangle, UiColors.ScrollbarThumb);
        }

        public bool IsPointerOverInteractiveElement(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            if (shopService.GetSellSummary(world).TotalValue > 0 && GetSellAllButtonRectangle(viewportWidth, viewportHeight).Contains(mousePosition))
            {
                return true;
            }

            if (shopService.GetOreSellSummary(world).TotalValue > 0 && GetSellOresButtonRectangle(viewportWidth, viewportHeight).Contains(mousePosition))
            {
                return true;
            }

            return false;
        }

        public void Close(ModelWorld world)
        {
            isOpen = false;
        }

        private Rectangle GetPanelRectangle(int viewportWidth, int viewportHeight)
        {
            return UiOverlayLayout.GetCenteredPanelRectangle(PanelWidthValue, PanelHeight, viewportWidth, viewportHeight, panelOffsetX);
        }
    }
}
