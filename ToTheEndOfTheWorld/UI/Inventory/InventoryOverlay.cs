using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete;
using ModelLibrary.Concrete.Grids;
using System;
using ToTheEndOfTheWorld.Context.StaticRepositories;
using ToTheEndOfTheWorld.Gameplay;

namespace ToTheEndOfTheWorld.UI.Inventory
{
    public sealed class InventoryOverlay : IGameOverlay
    {
        private readonly Grid craftingGrid = new(new Vector2(0, 0), new GridBox[3, 3]);
        private readonly GridBox craftOutputSlot = new(null, 0);
        private readonly InventoryService inventoryService;
        private readonly CraftingService craftingService;
        private readonly InventoryInteractionController interactionController = new();
        private readonly InventoryItemTextureResolver textureResolver;
        private Texture2D pixelTexture;
        private SpriteFont textFont;
        private InventoryLayout currentLayout;
        private bool isOpen;

        public InventoryOverlay(InventoryService inventoryService, CraftingService craftingService, WorldElementsRepository blocks, GameItemsRepository items)
        {
            this.inventoryService = inventoryService;
            this.craftingService = craftingService;
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

            if (!isOpen)
            {
                return;
            }

            currentLayout = InventoryLayoutCalculator.Create(viewportWidth, viewportHeight, world.Player.Inventory.Items.InternalGrid);
            interactionController.Update(currentMouseState, previousMouseState, currentLayout, world.Player.Inventory.Items.InternalGrid, craftingGrid, craftOutputSlot, craftingService);
        }

        public void Draw(SpriteBatch spriteBatch, World world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            var inventory = world.Player.Inventory;
            spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, viewportWidth, viewportHeight), Color.Black * 0.45f);
            spriteBatch.Draw(pixelTexture, currentLayout.PanelRectangle, new Color(24, 26, 34));
            DrawRectangleOutline(spriteBatch, currentLayout.PanelRectangle, 3, new Color(94, 102, 120));

            spriteBatch.DrawString(textFont, inventory.Name, new Vector2(currentLayout.PanelRectangle.X + 24, currentLayout.PanelRectangle.Y + 12), Color.White);
            spriteBatch.DrawString(textFont, $"Capacity: {inventory.SizeLimit}", new Vector2(currentLayout.PanelRectangle.X + 24, currentLayout.PanelRectangle.Y + 30), new Color(180, 188, 204));

            spriteBatch.DrawString(textFont, "Crafting", new Vector2(currentLayout.CraftingStart.X, currentLayout.CraftingStart.Y - 24), Color.White);
            DrawGrid(spriteBatch, craftingGrid.InternalGrid, currentLayout.CraftingStart.X, currentLayout.CraftingStart.Y, currentLayout.SlotSize, currentLayout.SlotSpacing);

            spriteBatch.DrawString(textFont, "Output", new Vector2(currentLayout.OutputSlotRectangle.X, currentLayout.OutputSlotRectangle.Y - 24), Color.White);
            DrawSlot(spriteBatch, craftOutputSlot, currentLayout.OutputSlotRectangle);
            spriteBatch.DrawString(textFont, ">", new Vector2(currentLayout.OutputArrowPosition.X, currentLayout.OutputArrowPosition.Y), Color.White);

            spriteBatch.Draw(pixelTexture, currentLayout.CraftButtonRectangle, new Color(70, 96, 74));
            DrawRectangleOutline(spriteBatch, currentLayout.CraftButtonRectangle, 2, new Color(137, 170, 142));
            DrawCenteredText(spriteBatch, "Craft", currentLayout.CraftButtonRectangle);

            spriteBatch.DrawString(textFont, "Inventory", new Vector2(currentLayout.InventoryStart.X, currentLayout.InventoryStart.Y - 24), Color.White);
            DrawGrid(spriteBatch, inventory.Items.InternalGrid, currentLayout.InventoryStart.X, currentLayout.InventoryStart.Y, currentLayout.SlotSize, currentLayout.SlotSpacing);

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
            spriteBatch.Draw(pixelTexture, slotRectangle, new Color(50, 56, 70));
            DrawRectangleOutline(spriteBatch, slotRectangle, 2, new Color(120, 129, 148));

            if (slot.Item == null || slot.Count <= 0)
            {
                return;
            }

            DrawItemTexture(spriteBatch, slot.Item, slotRectangle);
            DrawStackCount(spriteBatch, slot.Count, slotRectangle);
        }

        private void DrawHeldStack(SpriteBatch spriteBatch, AType item, int count, Point mousePosition)
        {
            const int heldSlotSize = 52;
            var heldRectangle = new Rectangle(mousePosition.X - (heldSlotSize / 2), mousePosition.Y - (heldSlotSize / 2), heldSlotSize, heldSlotSize);
            spriteBatch.Draw(pixelTexture, heldRectangle, new Color(50, 56, 70, 220));
            DrawRectangleOutline(spriteBatch, heldRectangle, 2, new Color(170, 180, 200));
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
                spriteBatch.DrawString(textFont, label, new Vector2(slotRectangle.X + 6, slotRectangle.Y + 4), Color.White);
                return;
            }

            var textureRectangle = FitTextureInside(slotRectangle, texture.Width, texture.Height, 8);
            spriteBatch.Draw(texture, textureRectangle, Color.White);
        }

        private void DrawStackCount(SpriteBatch spriteBatch, int count, Rectangle slotRectangle)
        {
            var countText = count.ToString();
            var countSize = textFont.MeasureString(countText);
            var countPosition = new Vector2(slotRectangle.Right - countSize.X - 6, slotRectangle.Bottom - countSize.Y - 4);
            spriteBatch.DrawString(textFont, countText, countPosition, Color.White);
        }

        private void DrawCenteredText(SpriteBatch spriteBatch, string text, Rectangle bounds)
        {
            var textSize = textFont.MeasureString(text);
            var textPosition = new Vector2(bounds.Center.X - (textSize.X / 2), bounds.Center.Y - (textSize.Y / 2));
            spriteBatch.DrawString(textFont, text, textPosition, Color.White);
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
