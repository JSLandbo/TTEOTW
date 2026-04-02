using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public sealed class InventoryOverlay(InventoryService inventoryService, CraftingService craftingService, InventoryItemUseService itemUseService, WorldElementsRepository blocks, GameItemsRepository items) : IGameOverlay
    {
        private const float HeaderTextScale = 1.15f;
        private const float SummaryTextScale = 1.0f;
        private const float ButtonTextScale = 1.0f;
        private const float StackTextScale = 0.95f;
        private readonly Grid craftingGrid = new(new Vector2(0, 0), new GridBox[3, 3]);
        private readonly GridBox craftOutputSlot = new(null, 0);
        private readonly InventoryInteractionController interactionController = new();
        private readonly InventoryHoverLabelResolver hoverLabelResolver = new();
        private readonly ItemTextureResolver textureResolver = new(blocks, items);
        private ItemSlotRenderer slotRenderer;
        private Texture2D pixelTexture;
        private Texture2D trashbinTexture;
        private Texture2D suicideTexture;
        private SpriteFont textFont;
        private InventoryLayout currentLayout;
        private bool isOpen;
        private bool selfDestructRequested;

        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;
        public bool HasHeldItem => interactionController.HasHeldItem;

        public bool ConsumeSelfDestructRequest()
        {
            bool requested = selfDestructRequested;
            selfDestructRequested = false;
            return requested;
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData([Color.White]);
            textFont = content.Load<SpriteFont>("File");
            trashbinTexture = content.Load<Texture2D>("General/Trashbin");
            suicideTexture = content.Load<Texture2D>("General/Suicide");
            slotRenderer = new ItemSlotRenderer(textureResolver, pixelTexture, textFont, StackTextScale);
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (UiInputHelper.WasJustPressed(currentKeyboardState, previousKeyboardState, Keys.I))
            {
                isOpen = !isOpen;

                if (!isOpen)
                {
                    interactionController.ReturnCraftingGridToInventory(inventoryService, world.Player.Inventory, craftingGrid);
                    interactionController.ReleaseHeldItem(inventoryService, world.Player.Inventory);
                }
            }
            else if (isOpen && UiInputHelper.WasJustPressed(currentKeyboardState, previousKeyboardState, Keys.Escape))
            {
                isOpen = false;
                interactionController.ReturnCraftingGridToInventory(inventoryService, world.Player.Inventory, craftingGrid);
                interactionController.ReleaseHeldItem(inventoryService, world.Player.Inventory);
            }

            if (!isOpen)
            {
                return;
            }

            currentLayout = InventoryLayoutCalculator.Create(viewportWidth, viewportHeight, world.Player.Inventory.Items.InternalGrid);

            if (UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState) && currentLayout.SelfDestructButtonRectangle.Contains(currentMouseState.Position))
            {
                selfDestructRequested = true;
                isOpen = false;
                interactionController.ClearHeldItemState();
                ClearGrid(craftingGrid.InternalGrid);
                craftOutputSlot.Item = null;
                craftOutputSlot.Count = 0;
                return;
            }

            interactionController.Update(currentMouseState, previousMouseState, currentLayout, world.Player.Inventory.Items.InternalGrid, craftingGrid, craftOutputSlot, craftingService, world, itemUseService, world.Player.Inventory, viewportWidth, viewportHeight);
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            AInventory inventory = world.Player.Inventory;
            UiDrawHelper.DrawScreenDim(spriteBatch, pixelTexture, viewportWidth, viewportHeight);
            spriteBatch.Draw(pixelTexture, currentLayout.PanelRectangle, new Color(24, 24, 24));
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, currentLayout.PanelRectangle, 2, new Color(92, 92, 92));
            spriteBatch.Draw(pixelTexture, currentLayout.HeaderRectangle, new Color(42, 42, 42));
            spriteBatch.Draw(pixelTexture, currentLayout.CraftingSectionRectangle, new Color(31, 31, 31));
            spriteBatch.Draw(pixelTexture, currentLayout.EquipmentSectionRectangle, new Color(34, 34, 34));
            spriteBatch.Draw(pixelTexture, currentLayout.InventorySectionRectangle, new Color(28, 28, 28));
            spriteBatch.Draw(pixelTexture, currentLayout.DividerRectangle, new Color(78, 78, 78));

            Vector2 headerTextPosition = new(currentLayout.PanelRectangle.X + 24, currentLayout.PanelRectangle.Y + 11);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, inventory.Name, headerTextPosition, Color.White, HeaderTextScale);

            string capacityText = $"Capacity: {inventory.SizeLimit}";
            Vector2 capacitySize = textFont.MeasureString(capacityText) * HeaderTextScale;
            GameTextRenderer.DrawBoldString(
                spriteBatch,
                textFont,
                capacityText,
                new Vector2(currentLayout.HeaderRectangle.Right - capacitySize.X - 24, currentLayout.PanelRectangle.Y + 11),
                new Color(185, 185, 185),
                HeaderTextScale);
            DrawSelfDestructButton(spriteBatch);

            DrawGrid(spriteBatch, craftingGrid.InternalGrid, currentLayout.CraftingStart.X, currentLayout.CraftingStart.Y, currentLayout.SlotSize, currentLayout.SlotSpacing);

            DrawSlot(spriteBatch, craftOutputSlot, currentLayout.OutputSlotRectangle);

            spriteBatch.Draw(pixelTexture, currentLayout.CraftButtonRectangle, new Color(86, 86, 86));
            bool isCraftButtonHovered = currentLayout.CraftButtonRectangle.Contains(interactionController.MousePosition);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, currentLayout.CraftButtonRectangle, isCraftButtonHovered);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, currentLayout.CraftButtonRectangle, 2, UiInteractionStyle.GetBorderColor(new Color(146, 146, 146), isCraftButtonHovered));
            UiDrawHelper.DrawCenteredText(spriteBatch, textFont, "Craft", currentLayout.CraftButtonRectangle, Color.White, ButtonTextScale);
            DrawEquipmentSlots(spriteBatch, world);
            DrawEquipmentSummary(spriteBatch, world);

            DrawGrid(spriteBatch, inventory.Items.InternalGrid, currentLayout.InventoryStart.X, currentLayout.InventoryStart.Y, currentLayout.SlotSize, currentLayout.SlotSpacing);
            DrawTrashBin(spriteBatch);

            if (interactionController.HeldItem != null && interactionController.HeldCount > 0)
            {
                DrawHeldStack(spriteBatch, interactionController.HeldItem, interactionController.HeldCount, interactionController.MousePosition);
            }
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

        private void DrawSlot(SpriteBatch spriteBatch, AGridBox slot, Rectangle slotRectangle)
        {
            bool isHovered = UiSlotInteractionHelper.CanInteractWithSlot(slot, interactionController.HasHeldItem) && slotRectangle.Contains(interactionController.MousePosition);
            slotRenderer.DrawGridSlot(spriteBatch, slotRectangle, slot, new Color(62, 62, 62), new Color(124, 124, 124), isHovered: isHovered);
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
            Color slotBackgroundColor = canEquipHeldItem ? new Color(78, 88, 78) : new Color(58, 58, 58);
            Color slotBorderColor = canEquipHeldItem ? new Color(162, 194, 162) : new Color(132, 132, 132);

            spriteBatch.Draw(pixelTexture, slotRectangle, slotBackgroundColor);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, slotRectangle, isHovered);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, slotRectangle, 2, UiInteractionStyle.GetBorderColor(slotBorderColor, isHovered));

            if (slotItem != null)
            {
                slotRenderer.DrawItem(spriteBatch, slotItem, slotRectangle, isHovered ? 0.95f : 1.0f);
            }
        }

        private void DrawEquipmentSummary(SpriteBatch spriteBatch, ModelWorld world)
        {
            const int panelPaddingX = 14;
            const int panelPaddingY = 10;
            int lineHeight = (int)System.Math.Ceiling(textFont.MeasureString("Hg").Y * SummaryTextScale) + 6;
            Rectangle summaryRectangle = new(
                currentLayout.EquipmentInfoRectangle.X,
                currentLayout.EquipmentInfoRectangle.Y,
                currentLayout.EquipmentInfoRectangle.Width - 16,
                currentLayout.EquipmentInfoRectangle.Height);

            spriteBatch.Draw(pixelTexture, summaryRectangle, new Color(40, 40, 40));
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, summaryRectangle, 1, new Color(96, 96, 96));

            int textY = summaryRectangle.Y + panelPaddingY;
            int separatorLeft = summaryRectangle.X + panelPaddingX;
            int separatorWidth = summaryRectangle.Width - (panelPaddingX * 2);

            DrawEquipmentSummaryTextLine(spriteBatch, world, EPlayerEquipmentSlotType.ThermalPlating, ref textY, summaryRectangle.X + panelPaddingX, lineHeight, separatorLeft, separatorWidth, drawSeparator: true);
            DrawEquipmentSummaryTextLine(spriteBatch, world, EPlayerEquipmentSlotType.Engine, ref textY, summaryRectangle.X + panelPaddingX, lineHeight, separatorLeft, separatorWidth, drawSeparator: true);
            DrawEquipmentSummaryTextLine(spriteBatch, world, EPlayerEquipmentSlotType.Inventory, ref textY, summaryRectangle.X + panelPaddingX, lineHeight, separatorLeft, separatorWidth, drawSeparator: true);
            DrawEquipmentSummaryTextLine(spriteBatch, world, EPlayerEquipmentSlotType.FuelTank, ref textY, summaryRectangle.X + panelPaddingX, lineHeight, separatorLeft, separatorWidth, drawSeparator: true);
            DrawEquipmentSummaryTextLine(spriteBatch, world, EPlayerEquipmentSlotType.Hull, ref textY, summaryRectangle.X + panelPaddingX, lineHeight, separatorLeft, separatorWidth, drawSeparator: true);
            DrawEquipmentSummaryTextLine(spriteBatch, world, EPlayerEquipmentSlotType.Drill, ref textY, summaryRectangle.X + panelPaddingX, lineHeight, separatorLeft, separatorWidth, drawSeparator: true);
            DrawEquipmentSummaryTextLine(spriteBatch, world, EPlayerEquipmentSlotType.Thruster, ref textY, summaryRectangle.X + panelPaddingX, lineHeight, separatorLeft, separatorWidth, drawSeparator: false);
        }

        private void DrawEquipmentSummaryTextLine(SpriteBatch spriteBatch, ModelWorld world, EPlayerEquipmentSlotType slotType, ref int textY, int textX, int lineHeight, int separatorLeft, int separatorWidth, bool drawSeparator)
        {
            AType equippedItem = itemUseService.GetEquippedItem(world, slotType);
            string line = itemUseService.GetSummaryText(slotType, equippedItem);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, line, new Vector2(textX, textY), new Color(244, 244, 244), SummaryTextScale);

            textY += lineHeight;
            if (drawSeparator)
            {
                spriteBatch.Draw(pixelTexture, new Rectangle(separatorLeft, textY - 3, separatorWidth, 1), new Color(76, 76, 76));
            }
        }

        private void DrawTrashBin(SpriteBatch spriteBatch)
        {
            bool canTrashHeldItem = interactionController.HeldItem != null;
            Color backgroundColor = canTrashHeldItem ? new Color(110, 58, 58) : new Color(58, 40, 40);
            Color borderColor = canTrashHeldItem ? new Color(210, 110, 110) : new Color(132, 92, 92);
            bool isHovered = currentLayout.TrashBinRectangle.Contains(interactionController.MousePosition);

            spriteBatch.Draw(pixelTexture, currentLayout.TrashBinRectangle, backgroundColor);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, currentLayout.TrashBinRectangle, isHovered);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, currentLayout.TrashBinRectangle, 2, UiInteractionStyle.GetBorderColor(borderColor, isHovered));
            DrawCenteredTexture(spriteBatch, trashbinTexture, currentLayout.TrashBinRectangle, canTrashHeldItem ? Color.White : Color.White * 0.55f);
        }

        private void DrawSelfDestructButton(SpriteBatch spriteBatch)
        {
            bool isHovered = currentLayout.SelfDestructButtonRectangle.Contains(interactionController.MousePosition);
            Color backgroundColor = isHovered ? new Color(120, 48, 48) : new Color(92, 36, 36);
            Color borderColor = isHovered ? new Color(226, 118, 118) : new Color(190, 92, 92);

            spriteBatch.Draw(pixelTexture, currentLayout.SelfDestructButtonRectangle, backgroundColor);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, currentLayout.SelfDestructButtonRectangle, isHovered);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, currentLayout.SelfDestructButtonRectangle, 2, borderColor);
            DrawCenteredTexture(spriteBatch, suicideTexture, currentLayout.SelfDestructButtonRectangle, Color.White);
        }

        public string GetHoverLabel(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            return hoverLabelResolver.Resolve(world, mousePosition, currentLayout, craftingGrid, craftOutputSlot, itemUseService, viewportWidth, viewportHeight);
        }

        public bool IsPointerOverInteractiveElement(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            return interactionController.IsPointerOverInteractiveElement(
                mousePosition,
                currentLayout,
                world.Player.Inventory.Items.InternalGrid,
                craftingGrid,
                craftOutputSlot,
                world.Player,
                itemUseService,
                viewportWidth,
                viewportHeight);
        }

        private static void ClearGrid(AGridBox[,] grid)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    grid[x, y].Item = null;
                    grid[x, y].Count = 0;
                }
            }
        }

        private void DrawHeldStack(SpriteBatch spriteBatch, AType item, int count, Point mousePosition)
        {
            const int heldSlotSize = 52;
            Rectangle heldRectangle = new(mousePosition.X - (heldSlotSize / 2), mousePosition.Y - (heldSlotSize / 2), heldSlotSize, heldSlotSize);
            spriteBatch.Draw(pixelTexture, heldRectangle, new Color(68, 68, 68, 220));
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, heldRectangle, 2, new Color(168, 168, 168));
            slotRenderer.DrawItemFitted(spriteBatch, item, heldRectangle);
            slotRenderer.DrawStackCount(spriteBatch, count, heldRectangle);
        }

        private static void DrawCenteredTexture(SpriteBatch spriteBatch, Texture2D texture, Rectangle bounds, Color color)
        {
            const int padding = 6;
            Rectangle textureBounds = new(bounds.X + padding, bounds.Y + padding, bounds.Width - (padding * 2), bounds.Height - (padding * 2));
            spriteBatch.Draw(texture, textureBounds, color);
        }

    }
}
