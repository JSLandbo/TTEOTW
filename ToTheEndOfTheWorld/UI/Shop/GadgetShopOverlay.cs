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
    public sealed class GadgetShopOverlay(GadgetShopService gadgetShopService, WorldElementsRepository blocks, GameItemsRepository items, Func<bool> hasHeldItem) : IInteractionOverlay
    {
        private const int HeaderHeight = 58;
        private const int ButtonWidth = 320;
        private const int ButtonHeight = 72;
        private const int GridColumns = 6;
        private const int GridRows = 6;
        private const int GridSlotSize = 64;
        private const int GridSpacing = 8;
        private const int GridPadding = 20;
        private const int ButtonTopMargin = 16;
        private const int ButtonBottomMargin = 20;
        private const int GridBottomPadding = 20;
        private const float TitleTextScale = 1.15f;
        private const float BodyTextScale = 1.0f;
        private const float ButtonTextScale = 1.0f;

        private static int GridWidth => (GridColumns * GridSlotSize) + ((GridColumns - 1) * GridSpacing);
        private static int GridHeight => (GridRows * GridSlotSize) + ((GridRows - 1) * GridSpacing);
        private static int PanelWidthValue => Math.Max(GridWidth, ButtonWidth) + (GridPadding * 2);
        private static int PanelHeight => HeaderHeight + ButtonTopMargin + ButtonHeight + ButtonBottomMargin + GridHeight + GridBottomPadding;

        private readonly ItemTextureResolver textureResolver = new(blocks, items);
        private Texture2D pixelTexture = null!;
        private SpriteFont textFont = null!;
        private ItemSlotRenderer slotRenderer = null!;
        private bool isOpen;
        private ABuilding currentBuilding = null!;
        private Point mousePosition;
        private int panelOffsetX;

        public EBuildingInteraction Action => EBuildingInteraction.GadgetShop;
        public bool IsOpen => isOpen;
        public bool BlocksGameplay => isOpen;
        public int PanelWidth => PanelWidthValue;

        public void Open(ABuilding building, int viewportWidth, int viewportHeight)
        {
            currentBuilding = building;
            panelOffsetX = 0;
            isOpen = true;
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

            mousePosition = currentMouseState.Position;

            // Block shop interaction when holding item
            if (hasHeldItem())
            {
                return;
            }

            if (UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState) &&
                GetBuyButtonRectangle(viewportWidth, viewportHeight).Contains(currentMouseState.Position))
            {
                gadgetShopService.TryBuyGadgetBelt(world);

                return;
            }

            if (world.Player.HasGadgetBelt &&
                UiInputHelper.WasLeftClicked(currentMouseState, previousMouseState) &&
                TryGetClickedShopSlot(currentMouseState.Position, viewportWidth, viewportHeight, out int slotX, out int slotY))
            {
                gadgetShopService.TryBuyGadget(world, currentBuilding, slotX, slotY);
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
            Rectangle buttonRectangle = GetBuyButtonRectangle(viewportWidth, viewportHeight);
            Rectangle gridRectangle = GetGridRectangle(panelRectangle);
            bool alreadyOwned = world.Player.HasGadgetBelt;
            bool canBuy = !alreadyOwned && world.Player.Cash >= gadgetShopService.GadgetBeltPriceValue;
            AGridBox[,] shopGrid = currentBuilding?.StorageInventory?.Items?.InternalGrid;

            if (currentBuilding?.ShowPlayerInventoryWhenOpen != true)
            {
                UiDrawHelper.DrawScreenDim(spriteBatch, pixelTexture, viewportWidth, viewportHeight);
            }

            spriteBatch.Draw(pixelTexture, panelRectangle, UiColors.PanelBackground);
            spriteBatch.Draw(pixelTexture, headerRectangle, UiColors.HeaderBackground);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, panelRectangle, 2, UiColors.PanelBorder);

            GameTextRenderer.DrawBoldString(spriteBatch, textFont, "Gadget Shop", new Vector2(panelRectangle.X + 20, panelRectangle.Y + 14), UiColors.TextTitle, TitleTextScale);

            spriteBatch.Draw(pixelTexture, buttonRectangle, canBuy ? UiColors.ActionButtonBackgroundGreenAlt : UiColors.ButtonBackgroundDisabled);
            bool isButtonHovered = canBuy && buttonRectangle.Contains(mousePosition);
            UiInteractionStyle.DrawHoverOverlay(spriteBatch, pixelTexture, buttonRectangle, isButtonHovered);
            UiDrawHelper.DrawRectangleOutline(spriteBatch, pixelTexture, buttonRectangle, 2, UiInteractionStyle.GetBorderColor(canBuy ? UiColors.ActionButtonBorderGreenAlt : UiColors.ButtonBorderDisabled, isButtonHovered));
            UiDrawHelper.DrawCenteredText(spriteBatch, textFont, alreadyOwned ? "Owned" : "Buy Gadget Belt", buttonRectangle, UiColors.TextButton, ButtonTextScale);

            if (!alreadyOwned)
            {
                GameTextRenderer.DrawBoldString(spriteBatch, textFont, $"Price: {gadgetShopService.GadgetBeltPriceValue:0}", new Vector2(buttonRectangle.X + 20, buttonRectangle.Bottom + 18), UiColors.TextButtonAlt, BodyTextScale);
            }

            DrawShopGrid(spriteBatch, gridRectangle, shopGrid, alreadyOwned, world);

            if (!alreadyOwned)
            {
                spriteBatch.Draw(pixelTexture, gridRectangle, Color.Black * 0.9f);
                UiDrawHelper.DrawCenteredText(spriteBatch, textFont, "Buy Gadget Belt to unlock shop", gridRectangle, UiColors.TextMuted, BodyTextScale);
            }
        }

        private Rectangle GetBuyButtonRectangle(int viewportWidth, int viewportHeight)
        {
            Rectangle panelRectangle = GetPanelRectangle(viewportWidth, viewportHeight);
            return new Rectangle(panelRectangle.X + ((panelRectangle.Width - ButtonWidth) / 2), panelRectangle.Y + HeaderHeight + ButtonTopMargin, ButtonWidth, ButtonHeight);
        }

        private static Rectangle GetGridRectangle(Rectangle panelRectangle)
        {
            return new Rectangle(
                panelRectangle.X + ((panelRectangle.Width - GridWidth) / 2),
                panelRectangle.Y + HeaderHeight + ButtonTopMargin + ButtonHeight + ButtonBottomMargin,
                GridWidth,
                GridHeight);
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
                    AGridBox slot = shopGrid?[x, y];
                    bool hasItem = slot?.Item != null;
                    bool canBuy = isEnabled && hasItem && world.Player.Cash >= slot.Item.Worth && gadgetShopService.CanBuyGadget(world, slot.Item);
                    bool isHovered = canBuy && slotRectangle.Contains(mousePosition);
                    slotRenderer.DrawGridSlot(
                        spriteBatch,
                        slotRectangle,
                        slot,
                        isEnabled ? UiColors.SlotBackground : UiColors.SlotBackgroundDisabled,
                        isEnabled ? UiColors.SlotBorder : UiColors.SlotBorderDisabled,
                        showCount: false,
                        isHovered: isHovered);
                }
            }
        }

        private Rectangle GetShopSlotRectangle(int viewportWidth, int viewportHeight, int slotX, int slotY)
        {
            Rectangle panelRectangle = GetPanelRectangle(viewportWidth, viewportHeight);
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
            if (hasHeldItem()) return false;

            bool canBuyBelt = !world.Player.HasGadgetBelt && world.Player.Cash >= gadgetShopService.GadgetBeltPriceValue;
            if (canBuyBelt && GetBuyButtonRectangle(viewportWidth, viewportHeight).Contains(mousePosition))
            {
                return true;
            }

            if (!world.Player.HasGadgetBelt || currentBuilding?.StorageInventory?.Items?.InternalGrid == null || !TryGetClickedShopSlot(mousePosition, viewportWidth, viewportHeight, out int slotX, out int slotY))
            {
                return false;
            }

            AGridBox slot = currentBuilding.StorageInventory.Items.InternalGrid[slotX, slotY];
            return slot.Item != null && world.Player.Cash >= slot.Item.Worth && gadgetShopService.CanBuyGadget(world, slot.Item);
        }

        public string GetHoverLabel(ModelWorld world, Point mousePosition, int viewportWidth, int viewportHeight)
        {
            if (!world.Player.HasGadgetBelt && GetBuyButtonRectangle(viewportWidth, viewportHeight).Contains(mousePosition))
            {
                return $"Gadget Belt {gadgetShopService.GadgetBeltPriceValue:0}";
            }

            if (!world.Player.HasGadgetBelt || currentBuilding?.StorageInventory?.Items?.InternalGrid == null || !TryGetClickedShopSlot(mousePosition, viewportWidth, viewportHeight, out int slotX, out int slotY))
            {
                return null;
            }

            AGridBox slot = currentBuilding.StorageInventory.Items.InternalGrid[slotX, slotY];
            return slot.Item != null ? $"{slot.Item.Name} {slot.Item.Worth:0}" : null;
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
