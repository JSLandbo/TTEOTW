using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.Text;

namespace ToTheEndOfTheWorld.UI.Shop
{
    public sealed class GadgetShopOverlay(InventoryService inventoryService, WorldElementsRepository blocks, GameItemsRepository items) : IInteractionOverlay
    {
        private const int PanelWidth = 620;
        private const int PanelHeight = 708;
        private const int HeaderHeight = 58;
        private const int ButtonWidth = 320;
        private const int ButtonHeight = 72;
        private const int GridColumns = 6;
        private const int GridRows = 6;
        private const int GridSlotSize = 64;
        private const int GridSpacing = 8;
        private const double GadgetBeltPrice = 10000.0;
        private const float TitleTextScale = 1.15f;
        private const float BodyTextScale = 1.0f;
        private const float ButtonTextScale = 1.0f;

        private readonly ItemTextureResolver textureResolver = new(blocks, items);
        private Texture2D pixelTexture = null!;
        private SpriteFont textFont = null!;
        private ItemSlotRenderer slotRenderer = null!;
        private bool isOpen;
        private ABuilding currentBuilding = null!;
        private Point mousePosition;

        public EBuildingInteraction Action => EBuildingInteraction.GadgetShop;
        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;

        public void Open(ABuilding building)
        {
            currentBuilding = building;
            isOpen = true;
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

            if (UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState) &&
                GetBuyButtonRectangle(viewportWidth, viewportHeight).Contains(currentMouseState.Position))
            {
                TryBuyGadgetBelt(world);

                return;
            }

            if (world.Player.HasGadgetBelt &&
                UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState) &&
                TryGetClickedShopSlot(currentMouseState.Position, viewportWidth, viewportHeight, out int slotX, out int slotY))
            {
                TryBuyGadget(world, slotX, slotY);
            }
        }

        public void Draw(SpriteBatch spriteBatch, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!isOpen)
            {
                return;
            }

            Rectangle panelRectangle = new((viewportWidth - PanelWidth) / 2, (viewportHeight - PanelHeight) / 2, PanelWidth, PanelHeight);
            Rectangle headerRectangle = new(panelRectangle.X, panelRectangle.Y, panelRectangle.Width, HeaderHeight);
            Rectangle buttonRectangle = GetBuyButtonRectangle(viewportWidth, viewportHeight);
            Rectangle gridRectangle = GetGridRectangle(panelRectangle);
            bool alreadyOwned = world.Player.HasGadgetBelt;
            bool canBuy = !alreadyOwned && world.Player.Cash >= GadgetBeltPrice;
            AGridBox[,] shopGrid = currentBuilding?.StorageGrid?.InternalGrid;

            UiDrawHelper.DrawScreenDim(spriteBatch, pixelTexture, viewportWidth, viewportHeight);
            spriteBatch.Draw(pixelTexture, panelRectangle, new Color(22, 22, 22));
            spriteBatch.Draw(pixelTexture, headerRectangle, new Color(44, 44, 44));
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, panelRectangle, 2, new Color(108, 108, 108));

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Gadget Shop", new Vector2(panelRectangle.X + 20, panelRectangle.Y + 14), new Color(244, 240, 229), TitleTextScale);

            spriteBatch.Draw(pixelTexture, buttonRectangle, canBuy ? new Color(92, 116, 82) : new Color(64, 64, 64));
            bool isButtonHovered = canBuy && buttonRectangle.Contains(mousePosition);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, buttonRectangle, isButtonHovered);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, buttonRectangle, 2, UiInteractionStyle.GetBorderColor(canBuy ? new Color(162, 196, 146) : new Color(110, 110, 110), isButtonHovered));
            UiDrawHelper.DrawCenteredText(spriteBatch, textFont, alreadyOwned ? "Owned" : "Buy Gadget Belt", buttonRectangle, new Color(246, 241, 232), ButtonTextScale);

            if (!alreadyOwned)
            {
                GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Price: {GadgetBeltPrice:0}", new Vector2(buttonRectangle.X + 20, buttonRectangle.Bottom + 18), new Color(230, 214, 166), BodyTextScale);
            }

            DrawShopGrid(spriteBatch, gridRectangle, shopGrid, alreadyOwned, world);

            if (!alreadyOwned)
            {
                spriteBatch.Draw(pixelTexture, gridRectangle, Color.Black * 0.9f);
                UiDrawHelper.DrawCenteredText(spriteBatch, textFont, "Buy Gadget Belt to unlock shop", gridRectangle, new Color(126, 126, 126), BodyTextScale);
            }
        }

        private static void TryBuyGadgetBelt(ModelWorld world)
        {
            if (world.Player.HasGadgetBelt || world.Player.Cash < GadgetBeltPrice)
            {
                return;
            }

            world.Player.HasGadgetBelt = true;
            world.Player.Cash -= GadgetBeltPrice;
        }

        private bool TryBuyGadget(ModelWorld world, int slotX, int slotY)
        {
            AGridBox[,] grid = currentBuilding?.StorageGrid?.InternalGrid;

            if (grid == null || slotX < 0 || slotX >= grid.GetLength(0) || slotY < 0 || slotY >= grid.GetLength(1))
            {
                return false;
            }

            AGridBox slot = grid[slotX, slotY];

            if (slot.Item == null || world.Player.Cash < slot.Item.Worth)
            {
                return false;
            }

            AType purchasedItem = items.Create(slot.Item.ID);

            if (!inventoryService.TryAdd(world.Player.Inventory, purchasedItem, 1))
            {
                return false;
            }

            world.Player.Cash -= slot.Item.Worth;

            return true;
        }

        private Rectangle GetBuyButtonRectangle(int viewportWidth, int viewportHeight)
        {
            return new Rectangle((viewportWidth - ButtonWidth) / 2, (viewportHeight - PanelHeight) / 2 + 74, ButtonWidth, ButtonHeight);
        }

        private static Rectangle GetGridRectangle(Rectangle panelRectangle)
        {
            int gridWidth = (GridColumns * GridSlotSize) + ((GridColumns - 1) * GridSpacing);
            int gridHeight = (GridRows * GridSlotSize) + ((GridRows - 1) * GridSpacing);

            return new Rectangle(
                panelRectangle.X + ((panelRectangle.Width - gridWidth) / 2),
                panelRectangle.Y + 206,
                gridWidth,
                gridHeight);
        }

        private bool TryGetClickedShopSlot(Point mousePosition, int viewportWidth, int viewportHeight, out int slotX, out int slotY)
        {
            return UiGridHitTestHelper.TryGetCoordinates(GridColumns, GridRows, mousePosition, (x, y) => GetShopSlotRectangle(viewportWidth, viewportHeight, x, y), out slotX, out slotY);
        }

        private void DrawShopGrid(SpriteBatch spriteBatch, Rectangle gridRectangle, AGridBox[,] shopGrid, bool isEnabled, ModelWorld world)
        {
            for (int y = 0; y < GridRows; y++)
            {
                for (int x = 0; x < GridColumns; x++)
                {
                    Rectangle slotRectangle = GetShopSlotRectangle(gridRectangle, x, y);
                    AGridBox slot = shopGrid?[x, y] ?? new GridBox(null, 0);
                    bool isHovered = isEnabled && slot.Item != null && world.Player.Cash >= slot.Item.Worth && slotRectangle.Contains(mousePosition);
                    slotRenderer.DrawGridSlot(
                        spriteBatch,
                        slotRectangle,
                        slot,
                        isEnabled ? new Color(44, 44, 44) : new Color(10, 10, 10),
                        isEnabled ? new Color(108, 108, 108) : new Color(20, 20, 20),
                        showCount: false,
                        isHovered: isHovered);
                }
            }
        }

        private Rectangle GetShopSlotRectangle(int viewportWidth, int viewportHeight, int slotX, int slotY)
        {
            Rectangle panelRectangle = new((viewportWidth - PanelWidth) / 2, (viewportHeight - PanelHeight) / 2, PanelWidth, PanelHeight);
            return GetShopSlotRectangle(GetGridRectangle(panelRectangle), slotX, slotY);
        }

        private static Rectangle GetShopSlotRectangle(Rectangle gridRectangle, int slotX, int slotY)
        {
            return new Rectangle(
                gridRectangle.X + (slotX * (GridSlotSize + GridSpacing)),
                gridRectangle.Y + (slotY * (GridSlotSize + GridSpacing)),
                GridSlotSize,
                GridSlotSize);
        }

        public bool IsPointerOverInteractiveElement(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            bool canBuyBelt = !world.Player.HasGadgetBelt && world.Player.Cash >= GadgetBeltPrice;
            if (canBuyBelt && GetBuyButtonRectangle(viewportWidth, viewportHeight).Contains(mousePosition))
            {
                return true;
            }

            if (!world.Player.HasGadgetBelt || currentBuilding?.StorageGrid?.InternalGrid == null || !TryGetClickedShopSlot(mousePosition, viewportWidth, viewportHeight, out int slotX, out int slotY))
            {
                return false;
            }

            AGridBox slot = currentBuilding.StorageGrid.InternalGrid[slotX, slotY];
            return slot.Item != null && world.Player.Cash >= slot.Item.Worth;
        }

        public string GetHoverLabel(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            if (!world.Player.HasGadgetBelt || currentBuilding?.StorageGrid?.InternalGrid == null || !TryGetClickedShopSlot(mousePosition, viewportWidth, viewportHeight, out int slotX, out int slotY))
            {
                return null;
            }

            AGridBox slot = currentBuilding.StorageGrid.InternalGrid[slotX, slotY];
            return slot.Item?.Name;
        }

        public void Close(ModelWorld world)
        {
            isOpen = false;
        }
    }
}
