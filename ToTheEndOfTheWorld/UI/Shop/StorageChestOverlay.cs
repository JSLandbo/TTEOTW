using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class StorageChestOverlay(InventoryService inventoryService, WorldElementsRepository blocks, GameItemsRepository items, Func<bool> hasHeldItem) : IInteractionOverlay
    {
        private const int PanelWidth = 620;
        private const int PanelHeight = 708;
        private const int HeaderHeight = 58;
        private const int GridSlotSize = 64;
        private const int GridSpacing = 8;
        private const int VisibleRows = 8;
        private const float TitleTextScale = 1.15f;
        private const float ButtonTextScale = 1.0f;
        private const int SortButtonWidth = 72;
        private const int SortButtonHeight = 36;

        private readonly ItemTextureResolver textureResolver = new(blocks, items);
        private Texture2D pixelTexture = null!;
        private SpriteFont textFont = null!;
        private ItemSlotRenderer slotRenderer = null!;
        private bool isOpen;
        private ABuilding currentBuilding = null!;
        private Point mousePosition;
        private int scrollOffset;
        private bool isTransferModeActive;

        public EBuildingInteraction Action => EBuildingInteraction.StorageChest;
        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;
        public bool IsTransferModeActive => isOpen && isTransferModeActive;

        public void Open(ABuilding building)
        {
            currentBuilding = building;
            isOpen = true;
            scrollOffset = 0;
        }

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

            mousePosition = currentMouseState.Position;
            isTransferModeActive = currentKeyboardState.IsKeyDown(Keys.LeftControl) || currentKeyboardState.IsKeyDown(Keys.RightControl);

            // Handle scroll
            Rectangle gridRectangle = GetGridRectangle(viewportWidth, viewportHeight);
            int scrollDelta = currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;
            int maxScrollOffset = Math.Max(0, StorageChestBuildingFactory.GridRows - VisibleRows);

            if (scrollDelta != 0 && gridRectangle.Contains(mousePosition))
            {
                scrollOffset -= Math.Sign(scrollDelta);
                scrollOffset = Math.Clamp(scrollOffset, 0, maxScrollOffset);
            }

            // Handle sort button click
            if (!hasHeldItem() && UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState))
            {
                Rectangle sortButtonRectangle = GetSortButtonRectangle(viewportWidth, viewportHeight);
                if (sortButtonRectangle.Contains(mousePosition))
                {
                    SortChestGrid();
                    return;
                }
            }

            // Handle CTRL+click on chest slots to transfer to inventory
            if (isTransferModeActive && UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState))
            {
                if (TryGetClickedChestSlot(mousePosition, viewportWidth, viewportHeight, out AGridBox clickedSlot) && clickedSlot?.Item != null)
                {
                    TryTransferFromChest(world, clickedSlot);
                }
            }
        }

        public bool TryTransferToChest(AGridBox sourceSlot)
        {
            if (!IsTransferModeActive || sourceSlot?.Item == null || sourceSlot.Count <= 0 || currentBuilding?.StorageGrid == null)
            {
                return false;
            }

            int added = inventoryService.AddToGrid(currentBuilding.StorageGrid, sourceSlot.Item, sourceSlot.Count, StorageChestBuildingFactory.MaxStackSize);
            if (added > 0)
            {
                sourceSlot.Count -= added;
                return true;
            }

            return false;
        }

        public int AddToChest(AType item, int count)
        {
            if (item == null || count <= 0)
            {
                return 0;
            }

            if (currentBuilding?.StorageGrid == null)
            {
                return 0;
            }

            return inventoryService.AddToGrid(currentBuilding.StorageGrid, item, count, StorageChestBuildingFactory.MaxStackSize);
        }

        public bool TryTransferFromChest(ModelWorld world, AGridBox sourceSlot)
        {
            if (!IsTransferModeActive || sourceSlot?.Item == null || sourceSlot.Count <= 0)
            {
                return false;
            }

            // Try inventory first
            int addedToInventory = inventoryService.AddToInventory(world.Player.Inventory, sourceSlot.Item, sourceSlot.Count);
            if (addedToInventory > 0)
            {
                sourceSlot.Count -= addedToInventory;
            }

            // If still have items and player has gadget belt, try gadget slots
            if (sourceSlot.Count > 0 && world.Player.HasGadgetBelt)
            {
                int addedToGadgets = inventoryService.AddToInventory(world.Player.GadgetSlots, sourceSlot.Item, sourceSlot.Count);
                if (addedToGadgets > 0)
                {
                    sourceSlot.Count -= addedToGadgets;
                }
            }

            return addedToInventory > 0;
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen || currentBuilding?.StorageGrid == null)
            {
                return;
            }

            Rectangle panelRectangle = GetPanelRectangle(viewportWidth, viewportHeight);
            Rectangle headerRectangle = new(panelRectangle.X, panelRectangle.Y, panelRectangle.Width, HeaderHeight);
            Rectangle gridRectangle = GetGridRectangle(viewportWidth, viewportHeight);
            AGridBox[,] chestGrid = currentBuilding.StorageGrid.InternalGrid;

            if (currentBuilding.ShowPlayerInventoryWhenOpen != true)
            {
                UiDrawHelper.DrawScreenDim(spriteBatch, pixelTexture, viewportWidth, viewportHeight);
            }

            spriteBatch.Draw(pixelTexture, panelRectangle, UiColors.PanelBackground);
            spriteBatch.Draw(pixelTexture, headerRectangle, UiColors.HeaderBackground);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, panelRectangle, 2, UiColors.PanelBorder);

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, currentBuilding.Name, new Vector2(panelRectangle.X + 20, panelRectangle.Y + 14), UiColors.TextTitle, TitleTextScale);

            DrawChestGrid(spriteBatch, gridRectangle, chestGrid);
            DrawScrollBar(spriteBatch, gridRectangle);
            DrawSortButton(spriteBatch, viewportWidth, viewportHeight);
        }

        private void DrawSortButton(SpriteBatch spriteBatch, int viewportWidth, int viewportHeight)
        {
            bool canSort = !hasHeldItem();
            Rectangle sortButtonRectangle = GetSortButtonRectangle(viewportWidth, viewportHeight);
            bool isHovered = canSort && sortButtonRectangle.Contains(mousePosition);
            Color backgroundColor = canSort ? UiColors.SortButtonBackground : UiColors.SortButtonBackgroundDisabled;
            Color borderColor = canSort ? UiColors.SortButtonBorder : UiColors.SortButtonBorderDisabled;
            Color textColor = canSort ? Color.White : Color.White * 0.55f;

            spriteBatch.Draw(pixelTexture, sortButtonRectangle, backgroundColor);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, sortButtonRectangle, isHovered);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, sortButtonRectangle, 2, UiInteractionStyle.GetBorderColor(borderColor, isHovered));
            UiDrawHelper.DrawCenteredText(spriteBatch, textFont, "Sort", sortButtonRectangle, textColor, ButtonTextScale);
        }

        private void DrawChestGrid(SpriteBatch spriteBatch, Rectangle gridRectangle, AGridBox[,] chestGrid)
        {
            for (int visibleY = 0; visibleY < VisibleRows; visibleY++)
            {
                int actualY = scrollOffset + visibleY;
                if (actualY >= StorageChestBuildingFactory.GridRows) break;

                for (int x = 0; x < StorageChestBuildingFactory.GridColumns; x++)
                {
                    Rectangle slotRectangle = GetSlotRectangle(gridRectangle, x, visibleY);
                    AGridBox slot = chestGrid[x, actualY];
                    bool isHovered = slotRectangle.Contains(mousePosition);
                    slotRenderer.DrawGridSlot(spriteBatch, slotRectangle, slot, UiColors.SlotBackground, UiColors.SlotBorder, showCount: true, isHovered: isHovered);
                }
            }
        }

        private void DrawScrollBar(SpriteBatch spriteBatch, Rectangle gridRectangle)
        {
            int totalRows = StorageChestBuildingFactory.GridRows;
            if (totalRows <= VisibleRows) return;

            Rectangle trackRectangle = new(gridRectangle.Right + 4, gridRectangle.Y, 6, gridRectangle.Height);
            int thumbHeight = Math.Max(24, (int)(trackRectangle.Height * (VisibleRows / (float)totalRows)));
            int maxScrollOffset = Math.Max(1, totalRows - VisibleRows);
            int thumbTravel = trackRectangle.Height - thumbHeight;
            int thumbY = trackRectangle.Y + (int)(thumbTravel * (scrollOffset / (float)maxScrollOffset));
            Rectangle thumbRectangle = new(trackRectangle.X, thumbY, trackRectangle.Width, thumbHeight);

            spriteBatch.Draw(pixelTexture, trackRectangle, UiColors.ScrollbarTrack);
            spriteBatch.Draw(pixelTexture, thumbRectangle, UiColors.ScrollbarThumb);
        }

        private Rectangle GetSlotRectangle(Rectangle gridRectangle, int slotX, int visibleY)
        {
            return new Rectangle(
                gridRectangle.X + (slotX * (GridSlotSize + GridSpacing)),
                gridRectangle.Y + (visibleY * (GridSlotSize + GridSpacing)),
                GridSlotSize,
                GridSlotSize);
        }

        public bool TryGetClickedChestSlot(Point position, int viewportWidth, int viewportHeight, out AGridBox slot)
        {
            slot = null;
            if (currentBuilding?.StorageGrid?.InternalGrid == null) return false;

            Rectangle gridRectangle = GetGridRectangle(viewportWidth, viewportHeight);

            if (!UiGridHitTestHelper.TryGetCoordinates(StorageChestBuildingFactory.GridColumns, VisibleRows, position, (x, y) => GetSlotRectangle(gridRectangle, x, y), out int slotX, out int visibleY))
            {
                return false;
            }

            int actualY = scrollOffset + visibleY;
            if (actualY >= StorageChestBuildingFactory.GridRows) return false;

            slot = currentBuilding.StorageGrid.InternalGrid[slotX, actualY];
            return true;
        }

        public int GetMaxStackSize() => StorageChestBuildingFactory.MaxStackSize;

        public bool IsPointerOverInteractiveElement(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            // Sort button is interactive when not holding item
            if (!hasHeldItem() && GetSortButtonRectangle(viewportWidth, viewportHeight).Contains(mousePosition))
            {
                return true;
            }

            // Chest slots are interactive like inventory slots
            if (TryGetClickedChestSlot(mousePosition, viewportWidth, viewportHeight, out AGridBox slot))
            {
                return UiSlotInteractionHelper.CanInteractWithSlot(slot, hasHeldItem());
            }

            return false;
        }

        public string GetHoverLabel(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            if (!TryGetClickedChestSlot(mousePosition, viewportWidth, viewportHeight, out AGridBox slot) || slot?.Item == null)
            {
                return null;
            }

            return slot.Item.Name;
        }

        public void Close(ModelWorld world)
        {
            isOpen = false;
        }

        private Rectangle GetPanelRectangle(int viewportWidth, int viewportHeight)
        {
            return UiOverlayLayout.GetCenteredPanelRectangle(PanelWidth, PanelHeight, viewportWidth, viewportHeight, currentBuilding);
        }

        private Rectangle GetGridRectangle(int viewportWidth, int viewportHeight)
        {
            Rectangle panelRectangle = GetPanelRectangle(viewportWidth, viewportHeight);
            int gridWidth = (StorageChestBuildingFactory.GridColumns * GridSlotSize) + ((StorageChestBuildingFactory.GridColumns - 1) * GridSpacing);
            int gridHeight = (VisibleRows * GridSlotSize) + ((VisibleRows - 1) * GridSpacing);

            return new Rectangle(
                panelRectangle.X + ((panelRectangle.Width - gridWidth - 10) / 2),
                panelRectangle.Y + HeaderHeight + 20,
                gridWidth,
                gridHeight);
        }

        private Rectangle GetSortButtonRectangle(int viewportWidth, int viewportHeight)
        {
            Rectangle panelRectangle = GetPanelRectangle(viewportWidth, viewportHeight);
            return new Rectangle(
                panelRectangle.Right - SortButtonWidth - 16,
                panelRectangle.Y + ((HeaderHeight - SortButtonHeight) / 2),
                SortButtonWidth,
                SortButtonHeight);
        }

        private void SortChestGrid()
        {
            if (currentBuilding?.StorageGrid == null) return;
            inventoryService.SortGridByName(currentBuilding.StorageGrid, StorageChestBuildingFactory.MaxStackSize);
        }
    }
}
