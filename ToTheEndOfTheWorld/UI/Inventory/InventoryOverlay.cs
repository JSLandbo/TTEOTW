using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public sealed class InventoryOverlay(InventoryService inventoryService, CraftingService craftingService, InventoryItemUseService itemUseService, WorldElementsRepository blocks, GameItemsRepository items, Func<bool> isShopOpen, Func<ModelWorld, AGridBox, bool> trySellSlot, Func<Point, int, int, (AGridBox slot, int maxStackSize)?> tryGetChestSlot = null, Func<AInventory> getOpenSecondaryInventory = null) : IGameOverlay
    {
        private const float HeaderTextScale = 1.15f;
        private const float ButtonTextScale = 1.0f;
        private const float StackTextScale = 0.95f;
        private const int ScrollbarWidth = 10;
        private readonly Grid craftingGrid = new(new Vector2(0, 0), new GridBox[3, 3]);
        private readonly GridBox craftOutputSlot = new(null, 0);
        private readonly InventoryInteractionController interactionController = new();
        private readonly InventoryHoverLabelResolver hoverLabelResolver = new();
        private readonly ItemTextureResolver textureResolver = new(blocks, items);
        private ItemSlotRenderer slotRenderer;
        private Texture2D pixelTexture;
        private Texture2D trashbinTexture;
        private SpriteFont textFont;
        private InventoryLayout currentLayout;
        private bool isOpen;
        private int panelXOffset;
        private int cachedViewportWidth;
        private int cachedViewportHeight;
        private int inventoryScrollOffset;

        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;
        public bool HasHeldItem => interactionController.HasHeldItem;

        public int PanelWidth => currentLayout.PanelRectangle.Width;

        public void Open(int viewportWidth, int viewportHeight, APlayer player)
        {
            isOpen = true;
            panelXOffset = 0;
            inventoryScrollOffset = 0;
            cachedViewportWidth = viewportWidth;
            cachedViewportHeight = viewportHeight;
            currentLayout = InventoryLayoutCalculator.Create(viewportWidth, viewportHeight, player.Inventory.Items.InternalGrid, panelXOffset);
        }

        public void SetPanelOffset(int offsetX)
        {
            panelXOffset = offsetX;
        }

        public void RefreshLayout(APlayer player)
        {
            currentLayout = InventoryLayoutCalculator.Create(cachedViewportWidth, cachedViewportHeight, player.Inventory.Items.InternalGrid, panelXOffset);
        }

        public bool ConsumeTrashSoundRequest()
        {
            return interactionController.ConsumeTrashRequest();
        }

        public bool ConsumeSelectionSoundRequest()
        {
            return interactionController.ConsumeSelectionRequest();
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData([Color.White]);
            textFont = content.Load<SpriteFont>("File");
            trashbinTexture = content.Load<Texture2D>("General/Trashbin");
            slotRenderer = new ItemSlotRenderer(textureResolver, pixelTexture, textFont, StackTextScale);
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen) return;

            // Recalculate layout if viewport changed
            if (viewportWidth != cachedViewportWidth || viewportHeight != cachedViewportHeight)
            {
                cachedViewportWidth = viewportWidth;
                cachedViewportHeight = viewportHeight;
                currentLayout = InventoryLayoutCalculator.Create(viewportWidth, viewportHeight, world.Player.Inventory.Items.InternalGrid, panelXOffset);
            }

            // Handle inventory scroll
            int totalRows = world.Player.Inventory.Items.InternalGrid.GetLength(1);
            if (totalRows > InventoryLayoutCalculator.VisibleInventoryRows)
            {
                int scrollDelta = currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;
                if (scrollDelta != 0 && currentLayout.InventorySectionRectangle.Contains(currentMouseState.Position))
                {
                    inventoryScrollOffset = Math.Clamp(inventoryScrollOffset - Math.Sign(scrollDelta), 0, totalRows - InventoryLayoutCalculator.VisibleInventoryRows);
                }
            }

            bool blockCrafting = isShopOpen();

            if (UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState)
                && currentLayout.SortButtonRectangle.Contains(currentMouseState.Position)
                && !interactionController.HasHeldItem)
            {
                inventoryService.SortByName(world.Player.Inventory);
                return;
            }

            InventoryInteractionContext ctx = new InventoryInteractionContext(
                currentLayout,
                world.Player.Inventory.Items.InternalGrid,
                craftingGrid,
                craftOutputSlot,
                craftingService,
                world,
                itemUseService,
                world.Player.Inventory,
                viewportWidth,
                viewportHeight,
                blockCrafting,
                inventoryScrollOffset,
                trySellSlot,
                tryGetChestSlot);

            interactionController.Update(currentMouseState, previousMouseState, ctx);
        }

        public void Close(ModelWorld world)
        {
            if (!isOpen)
            {
                return;
            }

            isOpen = false;
            panelXOffset = 0;

            foreach (AGridBox slot in InventoryService.EnumerateSlots(craftingGrid.InternalGrid))
            {
                ReturnSlotFromInventorySide(world.Player, slot);
            }

            ReturnSlotFromInventorySide(world.Player, craftOutputSlot);
            ReturnHeldItem(world.Player);
        }

        private void ReturnSlotFromInventorySide(APlayer player, AGridBox slot)
        {
            if (slot?.Item == null || slot.Count <= 0)
            {
                return;
            }

            inventoryService.PlaceSlotDuringClose(player.Inventory, getOpenSecondaryInventory?.Invoke(), player.HasGadgetBelt ? player.GadgetSlots : null, slot);
        }

        private void ReturnHeldItem(APlayer player)
        {
            if (!interactionController.TryTakeHeldItem(out AType heldItem, out int heldCount, out bool prefersChestReturn))
            {
                return;
            }

            GridBox heldSlot = new(heldItem, heldCount);
            AInventory secondaryInventory = getOpenSecondaryInventory?.Invoke();

            if (prefersChestReturn && secondaryInventory != null)
            {
                inventoryService.PlaceSlotDuringClose(secondaryInventory, player.Inventory, player.HasGadgetBelt ? player.GadgetSlots : null, heldSlot);
                return;
            }

            inventoryService.PlaceSlotDuringClose(player.Inventory, secondaryInventory, player.HasGadgetBelt ? player.GadgetSlots : null, heldSlot);
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            AInventory inventory = world.Player.Inventory;
            spriteBatch.Draw(pixelTexture, currentLayout.PanelRectangle, UiColors.PanelBackgroundLight);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, currentLayout.PanelRectangle, 2, UiColors.PanelBorderLight);
            spriteBatch.Draw(pixelTexture, currentLayout.HeaderRectangle, UiColors.HeaderBackgroundAlt);
            spriteBatch.Draw(pixelTexture, currentLayout.CraftingSectionRectangle, UiColors.SectionBackgroundAlt);
            spriteBatch.Draw(pixelTexture, currentLayout.EquipmentSectionRectangle, UiColors.SectionBackgroundAlt2);
            spriteBatch.Draw(pixelTexture, currentLayout.InventorySectionRectangle, UiColors.SectionBackground);
            spriteBatch.Draw(pixelTexture, currentLayout.DividerRectangle, UiColors.Divider);

            Vector2 headerTextPosition = new(currentLayout.PanelRectangle.X + 24, currentLayout.PanelRectangle.Y + 11);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, inventory.Name, headerTextPosition, Color.White, HeaderTextScale);

            DrawGrid(spriteBatch, craftingGrid.InternalGrid, currentLayout.CraftingStart.X, currentLayout.CraftingStart.Y, currentLayout.SlotSize, currentLayout.SlotSpacing);

            DrawSlot(spriteBatch, craftOutputSlot, currentLayout.OutputSlotRectangle);

            spriteBatch.Draw(pixelTexture, currentLayout.CraftButtonRectangle, UiColors.ButtonBackground);
            bool isCraftButtonHovered = currentLayout.CraftButtonRectangle.Contains(interactionController.MousePosition);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, currentLayout.CraftButtonRectangle, isCraftButtonHovered);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, currentLayout.CraftButtonRectangle, 2, UiInteractionStyle.GetBorderColor(UiColors.ButtonBorder, isCraftButtonHovered));
            UiDrawHelper.DrawCenteredText(spriteBatch, textFont, "Craft", currentLayout.CraftButtonRectangle, Color.White, ButtonTextScale);
            DrawEquipmentSlots(spriteBatch, world);
            DrawInventoryGrid(spriteBatch, inventory.Items.InternalGrid);
            DrawSortButton(spriteBatch);
            DrawTrashBin(spriteBatch);
        }

        public void DrawHeldItemOnTop(SpriteBatch spriteBatch)
        {
            if (!isOpen || interactionController.HeldItem == null || interactionController.HeldCount <= 0)
            {
                return;
            }

            DrawHeldStack(spriteBatch, interactionController.HeldItem, interactionController.HeldCount, interactionController.MousePosition);
        }

        private void DrawGrid(SpriteBatch spriteBatch, AGridBox[,] grid, int startX, int startY, int slotSize, int slotSpacing)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    int slotX = startX + (x * (slotSize + slotSpacing));
                    int slotY = startY + (y * (slotSize + slotSpacing));
                    DrawSlot(spriteBatch, grid[x, y], new Rectangle(slotX, slotY, slotSize, slotSize));
                }
            }
        }

        private void DrawInventoryGrid(SpriteBatch spriteBatch, AGridBox[,] grid)
        {
            int totalRows = grid.GetLength(1);
            int visibleRows = Math.Min(InventoryLayoutCalculator.VisibleInventoryRows, totalRows);

            for (int visibleY = 0; visibleY < visibleRows; visibleY++)
            {
                int actualY = inventoryScrollOffset + visibleY;
                if (actualY >= totalRows) break;

                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    int slotX = currentLayout.InventoryStart.X + (x * (currentLayout.SlotSize + currentLayout.SlotSpacing));
                    int slotY = currentLayout.InventoryStart.Y + (visibleY * (currentLayout.SlotSize + currentLayout.SlotSpacing));
                    DrawSlot(spriteBatch, grid[x, actualY], new Rectangle(slotX, slotY, currentLayout.SlotSize, currentLayout.SlotSize));
                }
            }

            if (totalRows > InventoryLayoutCalculator.VisibleInventoryRows) DrawInventoryScrollbar(spriteBatch, totalRows);
        }

        private void DrawInventoryScrollbar(SpriteBatch spriteBatch, int totalRows)
        {
            int visibleRows = Math.Min(InventoryLayoutCalculator.VisibleInventoryRows, totalRows);
            int gridHeight = (visibleRows * currentLayout.SlotSize) + ((visibleRows - 1) * currentLayout.SlotSpacing);
            int maxScrollOffset = Math.Max(1, totalRows - InventoryLayoutCalculator.VisibleInventoryRows);

            Rectangle trackRectangle = new(currentLayout.InventorySectionRectangle.Right - ScrollbarWidth - 4, currentLayout.InventoryStart.Y, 6, gridHeight);
            int thumbHeight = Math.Max(24, (int)(trackRectangle.Height * ((float)visibleRows / totalRows)));
            int thumbY = trackRectangle.Y + (int)((trackRectangle.Height - thumbHeight) * ((float)inventoryScrollOffset / maxScrollOffset));

            spriteBatch.Draw(pixelTexture, trackRectangle, UiColors.ScrollbarTrack);
            spriteBatch.Draw(pixelTexture, new Rectangle(trackRectangle.X, thumbY, trackRectangle.Width, thumbHeight), UiColors.ScrollbarThumb);
        }

        private void DrawSlot(SpriteBatch spriteBatch, AGridBox slot, Rectangle slotRectangle)
        {
            bool isHovered = UiSlotInteractionHelper.CanInteractWithSlot(slot, interactionController.HasHeldItem) && slotRectangle.Contains(interactionController.MousePosition);
            slotRenderer.DrawGridSlot(spriteBatch, slotRectangle, slot, UiColors.SlotBackgroundInventory, UiColors.SlotBorderInventory, isHovered: isHovered);
        }

        private void DrawEquipmentSlots(SpriteBatch spriteBatch, ModelWorld world)
        {
            foreach (EPlayerEquipmentSlotType slotType in Enum.GetValues<EPlayerEquipmentSlotType>())
            {
                DrawEquipmentSlot(spriteBatch, world, slotType);
            }
        }

        private void DrawEquipmentSlot(SpriteBatch spriteBatch, ModelWorld world, EPlayerEquipmentSlotType slotType)
        {
            Rectangle slotRectangle = currentLayout.GetEquipmentSlotRectangle(slotType);
            AType slotItem = itemUseService.GetEquippedItem(world, slotType);
            bool canEquipHeldItem = interactionController.HeldItem != null && itemUseService.CanEquip(interactionController.HeldItem, slotType);
            bool isHovered = slotRectangle.Contains(interactionController.MousePosition);
            Color slotBackgroundColor = canEquipHeldItem ? UiColors.SlotBackgroundEquipmentHighlight : UiColors.SlotBackgroundEquipment;
            Color slotBorderColor = canEquipHeldItem ? UiColors.SlotBorderEquipmentHighlight : UiColors.SlotBorderEquipment;

            spriteBatch.Draw(pixelTexture, slotRectangle, slotBackgroundColor);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, slotRectangle, isHovered);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, slotRectangle, 2, UiInteractionStyle.GetBorderColor(slotBorderColor, isHovered));

            if (slotItem != null)
            {
                slotRenderer.DrawItem(spriteBatch, slotItem, slotRectangle, isHovered ? 0.95f : 1.0f);
            }
        }

        private void DrawTrashBin(SpriteBatch spriteBatch)
        {
            bool canTrashHeldItem = interactionController.HeldItem != null;
            Color backgroundColor = canTrashHeldItem ? UiColors.TrashButtonBackground : UiColors.TrashButtonBackgroundDisabled;
            Color borderColor = canTrashHeldItem ? UiColors.TrashButtonBorder : UiColors.TrashButtonBorderDisabled;
            bool isHovered = currentLayout.TrashBinRectangle.Contains(interactionController.MousePosition);

            spriteBatch.Draw(pixelTexture, currentLayout.TrashBinRectangle, backgroundColor);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, currentLayout.TrashBinRectangle, isHovered);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, currentLayout.TrashBinRectangle, 2, UiInteractionStyle.GetBorderColor(borderColor, isHovered));
            DrawCenteredTexture(spriteBatch, trashbinTexture, currentLayout.TrashBinRectangle, canTrashHeldItem ? Color.White : Color.White * 0.55f);
        }

        private void DrawSortButton(SpriteBatch spriteBatch)
        {
            bool canSort = !interactionController.HasHeldItem;
            bool isHovered = canSort && currentLayout.SortButtonRectangle.Contains(interactionController.MousePosition);
            Color backgroundColor = canSort ? UiColors.SortButtonBackground : UiColors.SortButtonBackgroundDisabled;
            Color borderColor = canSort ? UiColors.SortButtonBorder : UiColors.SortButtonBorderDisabled;
            Color textColor = canSort ? Color.White : Color.White * 0.55f;

            spriteBatch.Draw(pixelTexture, currentLayout.SortButtonRectangle, backgroundColor);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, currentLayout.SortButtonRectangle, isHovered);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, currentLayout.SortButtonRectangle, 2, UiInteractionStyle.GetBorderColor(borderColor, isHovered));
            UiDrawHelper.DrawCenteredText(spriteBatch, textFont, "Sort", currentLayout.SortButtonRectangle, textColor, ButtonTextScale);
        }

        public string GetHoverLabel(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            return hoverLabelResolver.Resolve(world, mousePosition, currentLayout, craftingGrid, craftOutputSlot, itemUseService, viewportWidth, viewportHeight, inventoryScrollOffset);
        }

        public bool IsPointerOverInteractiveElement(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            InventoryInteractionContext ctx = new InventoryInteractionContext(
                currentLayout,
                world.Player.Inventory.Items.InternalGrid,
                craftingGrid,
                craftOutputSlot,
                craftingService,
                world,
                itemUseService,
                world.Player.Inventory,
                viewportWidth,
                viewportHeight,
                isShopOpen(),
                inventoryScrollOffset,
                trySellSlot,
                tryGetChestSlot);

            return interactionController.IsPointerOverInteractiveElement(mousePosition, ctx);
        }

        private void DrawHeldStack(SpriteBatch spriteBatch, AType item, int count, Point mousePosition)
        {
            int heldSlotSize = currentLayout.SlotSize;
            Rectangle heldRectangle = new(mousePosition.X - (heldSlotSize / 2), mousePosition.Y - (heldSlotSize / 2), heldSlotSize, heldSlotSize);
            slotRenderer.DrawItem(spriteBatch, item, heldRectangle);

            if (item.Stackable)
            {
                slotRenderer.DrawStackCount(spriteBatch, count, heldRectangle);
            }
        }

        private static void DrawCenteredTexture(SpriteBatch spriteBatch, Texture2D texture, Rectangle bounds, Color color)
        {
            const int padding = 6;
            Rectangle textureBounds = new(bounds.X + padding, bounds.Y + padding, bounds.Width - (padding * 2), bounds.Height - (padding * 2));
            spriteBatch.Draw(texture, textureBounds, color);
        }
    }
}
