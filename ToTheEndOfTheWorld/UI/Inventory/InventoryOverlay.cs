using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete;
using ModelLibrary.Concrete.Grids;
using System;
using ToTheEndOfTheWorld.Context;
using ToTheEndOfTheWorld.Gameplay;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public sealed class InventoryOverlay : IGameOverlay
    {
        private const float HeaderTextScale = 1.35f;
        private const float SummaryTextScale = 1.15f;
        private const float ButtonTextScale = 1.15f;
        private const float StackTextScale = 1.05f;
        private readonly Grid craftingGrid = new(new Vector2(0, 0), new GridBox[3, 3]);
        private readonly GridBox craftOutputSlot = new(null, 0);
        private readonly InventoryService inventoryService;
        private readonly CraftingService craftingService;
        private readonly InventoryItemUseService itemUseService;
        private readonly InventoryInteractionController interactionController = new();
        private readonly InventoryItemTextureResolver textureResolver;
        private Texture2D pixelTexture;
        private SpriteFont textFont;
        private InventoryLayout currentLayout;
        private bool isOpen;

        public InventoryOverlay(InventoryService inventoryService, CraftingService craftingService, InventoryItemUseService itemUseService, WorldElementsRepository blocks, GameItemsRepository items)
        {
            this.inventoryService = inventoryService;
            this.craftingService = craftingService;
            this.itemUseService = itemUseService;
            textureResolver = new InventoryItemTextureResolver(blocks, items);
        }

        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            pixelTexture = new Texture2D(graphicsDevice, 1, 1);
            pixelTexture.SetData(new[] { Color.White });
            textFont = content.Load<SpriteFont>("Fonts/text");
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState, MouseState currentMouseState, MouseState previousMouseState, World world, int viewportWidth, int viewportHeight)
        {
            if (WasJustPressed(currentKeyboardState, previousKeyboardState, Keys.I))
            {
                isOpen = !isOpen;

                if (!isOpen)
                {
                    interactionController.ReturnCraftingGridToInventory(inventoryService, world.Player.Inventory, craftingGrid);
                    interactionController.ReleaseHeldItem(inventoryService, world.Player.Inventory);
                }
            }
            else if (isOpen && WasJustPressed(currentKeyboardState, previousKeyboardState, Keys.Escape))
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
            interactionController.Update(currentMouseState, previousMouseState, currentLayout, world.Player.Inventory.Items.InternalGrid, craftingGrid, craftOutputSlot, craftingService, world, itemUseService, world.Player.Inventory);
        }

        public void Draw(SpriteBatch spriteBatch, World world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            var inventory = world.Player.Inventory;
            spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, viewportWidth, viewportHeight), Color.Black * 0.45f);
            spriteBatch.Draw(pixelTexture, currentLayout.PanelRectangle, new Color(24, 24, 24));
            DrawRectangleOutline(spriteBatch, currentLayout.PanelRectangle, 2, new Color(92, 92, 92));
            spriteBatch.Draw(pixelTexture, currentLayout.HeaderRectangle, new Color(42, 42, 42));
            spriteBatch.Draw(pixelTexture, currentLayout.CraftingSectionRectangle, new Color(31, 31, 31));
            spriteBatch.Draw(pixelTexture, currentLayout.EquipmentSectionRectangle, new Color(34, 34, 34));
            spriteBatch.Draw(pixelTexture, currentLayout.InventorySectionRectangle, new Color(28, 28, 28));
            spriteBatch.Draw(pixelTexture, currentLayout.DividerRectangle, new Color(78, 78, 78));

            var headerTextPosition = new Vector2(currentLayout.PanelRectangle.X + 24, currentLayout.PanelRectangle.Y + 11);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, inventory.Name, headerTextPosition, Color.White, HeaderTextScale);

            var capacityText = $"Capacity: {inventory.SizeLimit}";
            var capacitySize = textFont.MeasureString(capacityText) * HeaderTextScale;
            GameTextRenderer.DrawBoldString(
                spriteBatch,
                textFont,
                capacityText,
                new Vector2(currentLayout.HeaderRectangle.Right - capacitySize.X - 24, currentLayout.PanelRectangle.Y + 11),
                new Color(185, 185, 185),
                HeaderTextScale);

            DrawGrid(spriteBatch, craftingGrid.InternalGrid, currentLayout.CraftingStart.X, currentLayout.CraftingStart.Y, currentLayout.SlotSize, currentLayout.SlotSpacing);

            DrawSlot(spriteBatch, craftOutputSlot, currentLayout.OutputSlotRectangle);

            spriteBatch.Draw(pixelTexture, currentLayout.CraftButtonRectangle, new Color(86, 86, 86));
            DrawRectangleOutline(spriteBatch, currentLayout.CraftButtonRectangle, 2, new Color(146, 146, 146));
            DrawCenteredText(spriteBatch, "Craft", currentLayout.CraftButtonRectangle, ButtonTextScale);
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
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    var slotX = startX + (x * (slotSize + slotSpacing));
                    var slotY = startY + (y * (slotSize + slotSpacing));
                    DrawSlot(spriteBatch, grid[x, y], new Rectangle(slotX, slotY, slotSize, slotSize));
                }
            }
        }

        private void DrawSlot(SpriteBatch spriteBatch, AGridBox slot, Rectangle slotRectangle)
        {
            spriteBatch.Draw(pixelTexture, slotRectangle, new Color(62, 62, 62));
            DrawRectangleOutline(spriteBatch, slotRectangle, 2, new Color(124, 124, 124));

            if (slot.Item == null || slot.Count <= 0)
            {
                return;
            }

            DrawItemTexture(spriteBatch, slot.Item, slotRectangle);
            DrawStackCount(spriteBatch, slot.Count, slotRectangle);
        }

        private void DrawEquipmentSlots(SpriteBatch spriteBatch, World world)
        {
            DrawEquipmentSlot(spriteBatch, world, PlayerEquipmentSlotType.ThermalPlating);
            DrawEquipmentSlot(spriteBatch, world, PlayerEquipmentSlotType.Hull);
            DrawEquipmentSlot(spriteBatch, world, PlayerEquipmentSlotType.Drill);
            DrawEquipmentSlot(spriteBatch, world, PlayerEquipmentSlotType.Engine);
            DrawEquipmentSlot(spriteBatch, world, PlayerEquipmentSlotType.Inventory);
            DrawEquipmentSlot(spriteBatch, world, PlayerEquipmentSlotType.FuelTank);
            DrawEquipmentSlot(spriteBatch, world, PlayerEquipmentSlotType.Thruster);
        }

        private void DrawEquipmentSlot(SpriteBatch spriteBatch, World world, PlayerEquipmentSlotType slotType)
        {
            var slotRectangle = currentLayout.GetEquipmentSlotRectangle(slotType);
            var slotItem = itemUseService.GetEquippedItem(world, slotType);
            var canEquipHeldItem = interactionController.HeldItem != null && itemUseService.CanEquip(interactionController.HeldItem, slotType);
            spriteBatch.Draw(pixelTexture, slotRectangle, canEquipHeldItem ? new Color(78, 88, 78) : new Color(58, 58, 58));
            DrawRectangleOutline(spriteBatch, slotRectangle, 2, canEquipHeldItem ? new Color(162, 194, 162) : new Color(132, 132, 132));

            if (slotItem != null)
            {
                DrawItemTexture(spriteBatch, slotItem, slotRectangle);
            }
        }

        private void DrawEquipmentSummary(SpriteBatch spriteBatch, World world)
        {
            var textX = currentLayout.EquipmentInfoRectangle.X;
            var textY = currentLayout.EquipmentInfoRectangle.Y + 2;
            var lineHeight = (int)(textFont.LineSpacing * SummaryTextScale) + 12;

            DrawEquipmentSummaryLine(spriteBatch, world, PlayerEquipmentSlotType.ThermalPlating, ref textY, textX, lineHeight);
            DrawEquipmentSummaryLine(spriteBatch, world, PlayerEquipmentSlotType.Engine, ref textY, textX, lineHeight);
            DrawEquipmentSummaryLine(spriteBatch, world, PlayerEquipmentSlotType.Inventory, ref textY, textX, lineHeight);
            DrawEquipmentSummaryLine(spriteBatch, world, PlayerEquipmentSlotType.FuelTank, ref textY, textX, lineHeight);
            DrawEquipmentSummaryLine(spriteBatch, world, PlayerEquipmentSlotType.Hull, ref textY, textX, lineHeight);
            DrawEquipmentSummaryLine(spriteBatch, world, PlayerEquipmentSlotType.Drill, ref textY, textX, lineHeight);
            DrawEquipmentSummaryLine(spriteBatch, world, PlayerEquipmentSlotType.Thruster, ref textY, textX, lineHeight);
        }

        private void DrawEquipmentSummaryLine(SpriteBatch spriteBatch, World world, PlayerEquipmentSlotType slotType, ref int textY, int textX, int lineHeight)
        {
            var equippedItem = itemUseService.GetEquippedItem(world, slotType);
            var line = itemUseService.GetSummaryText(world, slotType, equippedItem);
            var tier = itemUseService.GetTierLabel(equippedItem);
            var accentColor = GetTierAccentColor(tier, equippedItem == null);
            var cardRectangle = new Rectangle(textX, textY, currentLayout.EquipmentInfoRectangle.Width, lineHeight - 4);

            spriteBatch.Draw(pixelTexture, cardRectangle, new Color(accentColor.R, accentColor.G, accentColor.B, (byte)42));
            DrawRectangleOutline(spriteBatch, cardRectangle, 1, new Color(accentColor.R, accentColor.G, accentColor.B, (byte)120));
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, line, new Vector2(textX + 10, textY + 6), new Color(244, 244, 244), SummaryTextScale);
            textY += lineHeight;
        }

        private void DrawTrashBin(SpriteBatch spriteBatch)
        {
            var canTrashHeldItem = interactionController.HeldItem != null;
            var backgroundColor = canTrashHeldItem ? new Color(110, 58, 58) : new Color(58, 40, 40);
            var borderColor = canTrashHeldItem ? new Color(210, 110, 110) : new Color(132, 92, 92);

            spriteBatch.Draw(pixelTexture, currentLayout.TrashBinRectangle, backgroundColor);
            DrawRectangleOutline(spriteBatch, currentLayout.TrashBinRectangle, 2, borderColor);
            DrawCenteredText(spriteBatch, "X", currentLayout.TrashBinRectangle, ButtonTextScale);
        }

        private static Color GetTierAccentColor(string tier, bool isEmpty)
        {
            if (isEmpty)
            {
                return new Color(102, 102, 102);
            }

            return tier.ToLowerInvariant() switch
            {
                "scrap" => new Color(132, 106, 82),
                "copper" => new Color(184, 114, 64),
                "iron" => new Color(148, 156, 166),
                "gold" => new Color(216, 184, 74),
                "crystal" => new Color(98, 196, 224),
                "diamond" => new Color(114, 218, 255),
                "radioactive" => new Color(106, 212, 92),
                "rainbow" => new Color(194, 108, 228),
                "mythril" => new Color(88, 224, 196),
                "adamant" => new Color(255, 112, 112),
                _ => new Color(124, 124, 124)
            };
        }

        private void DrawHeldStack(SpriteBatch spriteBatch, AType item, int count, Point mousePosition)
        {
            const int heldSlotSize = 52;
            var heldRectangle = new Rectangle(mousePosition.X - (heldSlotSize / 2), mousePosition.Y - (heldSlotSize / 2), heldSlotSize, heldSlotSize);
            spriteBatch.Draw(pixelTexture, heldRectangle, new Color(68, 68, 68, 220));
            DrawRectangleOutline(spriteBatch, heldRectangle, 2, new Color(168, 168, 168));
            DrawItemTexture(spriteBatch, item, heldRectangle);
            DrawStackCount(spriteBatch, count, heldRectangle);
        }

        private void DrawItemTexture(SpriteBatch spriteBatch, AType item, Rectangle slotRectangle)
        {
            var texture = textureResolver.Resolve(item);

            if (texture == null)
            {
                var fallbackRectangle = new Rectangle(slotRectangle.X + 6, slotRectangle.Y + 6, slotRectangle.Width - 12, slotRectangle.Height - 12);
                spriteBatch.Draw(pixelTexture, fallbackRectangle, new Color(118, 87, 53));

                var itemName = string.IsNullOrWhiteSpace(item.Name) ? $"ID{item.ID}" : item.Name;
                var label = itemName.Length > 3 ? itemName[..3].ToUpperInvariant() : itemName.ToUpperInvariant();
                GameTextRenderer.DrawBoldString(spriteBatch, textFont, label, new Vector2(slotRectangle.X + 6, slotRectangle.Y + 4), Color.White, StackTextScale);
                return;
            }

            var textureRectangle = FitTextureInside(slotRectangle, texture.Width, texture.Height, 8);
            spriteBatch.Draw(texture, textureRectangle, Color.White);
        }

        private void DrawStackCount(SpriteBatch spriteBatch, int count, Rectangle slotRectangle)
        {
            var countText = count.ToString();
            var countSize = textFont.MeasureString(countText) * StackTextScale;
            var countPosition = new Vector2(slotRectangle.Right - countSize.X - 6, slotRectangle.Bottom - countSize.Y - 4);
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, countText, countPosition, Color.White, StackTextScale);
        }

        private void DrawCenteredText(SpriteBatch spriteBatch, string text, Rectangle bounds, float scale)
        {
            var textSize = textFont.MeasureString(text) * scale;
            var textPosition = new Vector2(bounds.Center.X - (textSize.X / 2), bounds.Center.Y - (textSize.Y / 2));
            GameTextRenderer.DrawBoldString(spriteBatch, textFont, text, textPosition, Color.White, scale);
        }

        private void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rectangle, int thickness, Color color)
        {
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Bottom - thickness, rectangle.Width, thickness), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.X, rectangle.Y, thickness, rectangle.Height), color);
            spriteBatch.Draw(pixelTexture, new Rectangle(rectangle.Right - thickness, rectangle.Y, thickness, rectangle.Height), color);
        }

        private static Rectangle FitTextureInside(Rectangle bounds, int textureWidth, int textureHeight, int padding)
        {
            var availableWidth = bounds.Width - (padding * 2);
            var availableHeight = bounds.Height - (padding * 2);
            var scale = Math.Min((float)availableWidth / textureWidth, (float)availableHeight / textureHeight);
            var width = (int)(textureWidth * scale);
            var height = (int)(textureHeight * scale);
            var x = bounds.X + ((bounds.Width - width) / 2);
            var y = bounds.Y + ((bounds.Height - height) / 2);
            return new Rectangle(x, y, width, height);
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
