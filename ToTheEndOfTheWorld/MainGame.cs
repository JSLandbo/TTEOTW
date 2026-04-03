using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete;
using ModelLibrary.Concrete.Blocks;
using ModelLibrary.Concrete.PlayerShipComponents;
using ModelLibrary.Ids;
using ToTheEndOfTheWorld.Gameplay.Audio;
using ToTheEndOfTheWorld.Gameplay.Events;
using ToTheEndOfTheWorld.Gameplay.Graphics;
using ToTheEndOfTheWorld.UI;
using ToTheEndOfTheWorld.UI.Common;
using ToTheEndOfTheWorld.UI.Inventory;

namespace ToTheEndOfTheWorld
{
    public class MainGame : Game
    {
        // dotnet mgcb-editor

        private static readonly string GameTitle = "To The End Of The World";
        private static readonly string GameVersion = "V1.10";
        private static readonly int _pixels = 64;

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D sceneRenderTarget;
        private readonly ContentManager audioContent;
        private MiningInteractionsRepository miningInteractions;
        private WorldEffectsRepository worldEffects;
        private WorldEffectDefinitionsRepository worldEffectDefinitions;
        private WorldElementsRepository blocks;
        private GameItemsRepository items;
        private ModelWorld world;
        private readonly PlayerInputMapper inputMapper = new();
        private readonly PlayerFacingResolver playerFacingResolver = new();
        private readonly PlayerMovementSystem playerMovementSystem = new();
        private readonly PlayerFuelSystem playerFuelSystem = new();
        private readonly PlayerHeatSystem playerHeatSystem = new();
        private readonly PlayerHullSystem playerHullSystem = new();
        private readonly PlayerVerticalImpactService playerVerticalImpactService;
        private PlayerDeathSystem playerDeathSystem;
        private readonly WorldInteractionService worldInteractionService;
        private readonly WorldViewportService worldViewportService = new();
        private readonly GameEventBus eventBus = new();
        private readonly InventoryService inventoryService = new();
        private readonly ShopService shopService;
        private readonly GameAudioService audioService = new();
        private readonly UiWorld.DebugHudRenderer debugHudRenderer = new();
        private readonly UiWorld.GameplayHudRenderer gameplayHudRenderer = new();
        private UiWorld.GadgetBarRenderer gadgetBarRenderer;
        private readonly UiWorld.WorldInteractionRenderer worldInteractionRenderer = new();
        private GameplayAudioSystem gameplayAudioSystem;
        private WorldBlockDefinitionResolver worldBlockDefinitionResolver;
        private WorldBlockFactory worldBlockFactory;
        private PlayerWorldMovementResolver playerWorldMovementResolver;
        private PlayerMiningSystem playerMiningSystem;
        private PlayerConsumeableSystem playerConsumeableSystem;
        private UiWorld.PlayerShipRenderer playerShipRenderer;
        private UiManager uiManager;
        private InventoryOverlay inventoryOverlay;
        private UiWorld.DeathOverlay deathOverlay;
        private int logicalViewportWidth;
        private int logicalViewportHeight;
        private KeyboardState previousKeyboardState;
        private MouseState previousMouseState;
        private bool isApplyingResize;
        private Point uiMousePosition;
        private bool isUsingHandCursor;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            playerVerticalImpactService = new PlayerVerticalImpactService(playerHullSystem);
            worldInteractionService = new WorldInteractionService();
            shopService = new ShopService(eventBus);
            Content.RootDirectory = "Content/Graphics";
            audioContent = new ContentManager(Services, "Content");
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += HandleClientSizeChanged;
            Exiting += (_, _) =>
            {
                if (world != null)
                {
                    ContextHandler.SaveWorld(world);
                }
            };
        }

        protected override void Initialize()
        {
            blocks = new WorldElementsRepository(Content);
            items = new GameItemsRepository(Content);
            InventoryItemUseService inventoryItemUseService = new(inventoryService, items);
            FuelStationService fuelStationService = new(eventBus);
            GadgetShopService gadgetShopService = new(inventoryService, items, eventBus);
            SellShopBuildingFactory sellShopBuildingFactory = new();
            EquipmentShopBuildingFactory equipmentShopBuildingFactory = new(items);
            FuelStationBuildingFactory fuelStationBuildingFactory = new();
            GadgetShopBuildingFactory gadgetShopBuildingFactory = new(items);
            WorldBootstrapper worldBootstrapper = new(sellShopBuildingFactory, equipmentShopBuildingFactory, fuelStationBuildingFactory, gadgetShopBuildingFactory);
            worldBlockDefinitionResolver = new WorldBlockDefinitionResolver(blocks);
            worldBlockFactory = new WorldBlockFactory(worldBlockDefinitionResolver);
            CraftingService craftingService = new(new CraftingRecipeLibrary(items).CreateRecipes());
            _ = new WorldBlockLootSystem(eventBus, new BlockLootResolver(blocks), inventoryService);
            EquipmentShopService equipmentShopService = new(inventoryItemUseService, inventoryService, items, eventBus);
            uiManager = UiComposition.Create(inventoryService, craftingService, inventoryItemUseService, shopService, equipmentShopService, fuelStationService, gadgetShopService, blocks, items);
            inventoryOverlay = uiManager.GetOverlay<InventoryOverlay>();
            playerDeathSystem = new PlayerDeathSystem(items, worldViewportService);
            playerShipRenderer = new UiWorld.PlayerShipRenderer(items, _pixels);
            gadgetBarRenderer = new UiWorld.GadgetBarRenderer(blocks, items);
            gameplayAudioSystem = new GameplayAudioSystem(audioService, eventBus);

            int _blocksWide = (GraphicsDevice.DisplayMode.Width - (GraphicsDevice.DisplayMode.Width % _pixels)) / _pixels;
            int _blocksHigh = (GraphicsDevice.DisplayMode.Height - (GraphicsDevice.DisplayMode.Height % _pixels)) / _pixels;

            NormalizeVisibleTileCounts(ref _blocksWide, ref _blocksHigh);

            Window.Title = $"{GameTitle} {GameVersion}";
            graphics.PreferredBackBufferWidth = _blocksWide * _pixels;
            graphics.PreferredBackBufferHeight = _blocksHigh * _pixels;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            logicalViewportWidth = graphics.PreferredBackBufferWidth;
            logicalViewportHeight = graphics.PreferredBackBufferHeight;

            miningInteractions = new MiningInteractionsRepository();
            worldEffects = new WorldEffectsRepository();
            WorldBlockDamageService worldBlockDamageService = new(worldBlockDefinitionResolver, worldBlockFactory, miningInteractions, eventBus);
            playerWorldMovementResolver = new PlayerWorldMovementResolver(worldBlockDefinitionResolver, worldViewportService, playerVerticalImpactService, _pixels);
            playerMiningSystem = new PlayerMiningSystem(worldBlockDefinitionResolver, worldBlockDamageService, playerHeatSystem, playerHullSystem, playerFuelSystem, playerVerticalImpactService, _pixels);
            playerConsumeableSystem = new PlayerConsumeableSystem(worldBlockDamageService, worldEffects, eventBus);

            world = ContextHandler.LoadWorld();

            world ??= CreateNewWorld(_blocksWide, _blocksHigh);
            world.BlocksWide = _blocksWide;
            world.BlocksHigh = _blocksHigh;
            Vector2 initialWorldPosition = world.SavedPlayerWorldPosition != Vector2.Zero
                ? world.SavedPlayerWorldPosition
                : world.Player.Coordinates;
            worldViewportService.EnsurePadding(world, initialWorldPosition);
            world.SavedPlayerWorldPosition = worldViewportService.GetCenterWorldPosition(world);
            if (world.SpawnWorldPosition == Vector2.Zero)
            {
                world.SpawnWorldPosition = worldViewportService.GetCenterWorldPosition(world);
            }
            worldBootstrapper.EnsureInitialized(world);
            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();

            base.Initialize();
        }

        private ModelWorld CreateNewWorld(int _blocksWide, int _blocksHigh)
        {
            Player player = new(
                ThermalPlating: items.Create<ThermalPlating>(GameIds.Items.ThermalPlatings.Scrap),
                Engine: items.Create<Engine>(GameIds.Items.Engines.Scrap),
                Hull: items.Create<Hull>(GameIds.Items.Hulls.Scrap),
                Drill: items.Create<Drill>(GameIds.Items.Drills.Scrap),
                Inventory: items.Create<Inventory>(GameIds.Items.Containers.Scrap),
                Thruster: items.Create<Thruster>(GameIds.Items.Thrusters.Scrap),
                FuelTank: items.Create<FuelTank>(GameIds.Items.FuelTanks.Scrap)
            )
            {
                Coordinates = new Vector2((float)Math.Floor(_blocksWide / 2.0d), (float)Math.Floor(_blocksHigh / 2.0d)),
                Cash = 5000000000f // Starting allowance
            };

            return new ModelWorld(
                Player: player,
                Buildings: [],
                BlocksWide: _blocksWide,
                BlocksHigh: _blocksHigh,
                WorldRender: [],
                WorldTrails: []
            );
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sceneRenderTarget = new RenderTarget2D(GraphicsDevice, logicalViewportWidth, logicalViewportHeight);
            Texture2D youDiedTexture = Content.Load<Texture2D>("General/YouDiedText");
            deathOverlay = new UiWorld.DeathOverlay(youDiedTexture);
            debugHudRenderer.LoadContent(Content);
            gameplayHudRenderer.LoadContent(GraphicsDevice, Content);
            gadgetBarRenderer.LoadContent(GraphicsDevice, Content);
            worldInteractionRenderer.LoadContent(GraphicsDevice, Content);
            worldEffectDefinitions = new WorldEffectDefinitionsRepository(Content);
            uiManager.LoadContent(GraphicsDevice, Content);
            audioService.LoadContent(audioContent);
            audioService.PlayMusic(MusicTrack.MainTheme);
        }

        protected override void Update(GameTime gameTime)
        {
            double totalSeconds = gameTime.TotalGameTime.TotalSeconds;
            gameplayAudioSystem.SetTime(totalSeconds);
            TextureAnimationHelper.TotalSeconds = totalSeconds;
            KeyboardState keyboardState = IsActive
                ? Keyboard.GetState()
                : default;
            MouseState mouseState = CreateScaledMouseState(Mouse.GetState());
            uiMousePosition = mouseState.Position;

            if (UiInputHelper.WasJustPressed(keyboardState, previousKeyboardState, Keys.E))
            {
                if (inventoryOverlay?.IsOpen == true)
                {
                    inventoryOverlay.Close(world);
                }

                if (uiManager.BlocksGameplay)
                {
                    uiManager.CloseTopmost(world);
                }
                else
                {
                    worldInteractionService.TryHandleInteraction(uiManager, world);
                }
            }
            else if (UiInputHelper.WasJustPressed(keyboardState, previousKeyboardState, Keys.I))
            {
                if (inventoryOverlay?.IsOpen == true)
                {
                    inventoryOverlay.Close(world);
                }
                else
                {
                    if (uiManager.BlocksGameplay)
                    {
                        uiManager.CloseTopmost(world);
                    }

                    inventoryOverlay?.Open();
                }
            }
            else if (UiInputHelper.WasJustPressed(keyboardState, previousKeyboardState, Keys.Escape))
            {
                if (inventoryOverlay?.IsOpen == true)
                {
                    inventoryOverlay.Close(world);
                }
                if (uiManager.BlocksGameplay)
                {
                    uiManager.CloseTopmost(world);
                }
            }

            uiManager.Update(gameTime, keyboardState, previousKeyboardState, mouseState, previousMouseState, world, logicalViewportWidth, logicalViewportHeight);

            UpdateUiCursor(mouseState.Position);

            if (inventoryOverlay?.ConsumeSelfDestructRequest() == true)
            {
                playerVerticalImpactService.Clear();
                playerDeathSystem.SelfDestruct(world);
            }

            if (playerDeathSystem.TryHandleDeath(world))
            {
                gameplayAudioSystem.StopAllLoops();
                playerVerticalImpactService.Clear();
                playerDeathSystem.TryRespawnOnInput(world, keyboardState, previousKeyboardState);
                previousKeyboardState = keyboardState;
                previousMouseState = mouseState;
                base.Update(gameTime);

                return;
            }

            if (uiManager.BlocksGameplay)
            {
                gameplayAudioSystem.StopAllLoops();
                previousKeyboardState = keyboardState;
                previousMouseState = mouseState;
                base.Update(gameTime);

                return;
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            worldEffects.Update();
            playerVerticalImpactService.BeginFrame();

            int? consumeableSlotIndex = inputMapper.ReadTriggeredConsumeableSlot(keyboardState, previousKeyboardState);

            if (consumeableSlotIndex.HasValue)
            {
                playerConsumeableSystem.TryUse(world, consumeableSlotIndex.Value);
            }

            PlayerIntent intent = inputMapper.ReadPlayerIntent(keyboardState, previousKeyboardState);

            APlayer player = world.Player;
            Vector2 facingDirection = playerFacingResolver.Resolve(player, intent);

            player.ApplyIntent(intent.MovementInput, facingDirection);

            bool isGrounded = PlayerGroundingService.IsGrounded(world, player, worldBlockDefinitionResolver);

            if (!playerFuelSystem.CanAffordMovement(player, deltaTime, isGrounded))
            {
                player.MovementInput = Vector2.Zero;
            }
            if (PlayerThrusterUsageService.UsesThrustersForMovement(player, isGrounded) && !playerHeatSystem.CanUseThrusters(player))
            {
                player.MovementInput = Vector2.Zero;
            }

            playerMovementSystem.Update(player, deltaTime, isGrounded);

            int resolutionSteps = playerWorldMovementResolver.EstimateRequiredIterations(player);

            bool minedThisFrame = false;
            for (int i = 0; i < resolutionSteps; i++)
            {
                playerVerticalImpactService.BeginResolveStep(player);
                minedThisFrame |= playerMiningSystem.Update(world, player, deltaTime);

                if (!playerWorldMovementResolver.ResolveStep(world, player))
                {
                    break;
                }
            }
            player.Mining = minedThisFrame;

            if (PlayerThrusterUsageService.UsesThrustersForMovement(player, isGrounded))
            {
                playerHeatSystem.AddThrusterHeat(player, deltaTime);
            }

            playerHeatSystem.Update(player, deltaTime);
            playerFuelSystem.Update(player, deltaTime, isGrounded);
            playerHullSystem.Update(player, deltaTime);
            gameplayAudioSystem.Update(world, isGrounded);
            playerDeathSystem.TryHandleDeath(world);

            previousKeyboardState = keyboardState;
            previousMouseState = mouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(sceneRenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            UiWorld.WorldScreenTransform worldScreenTransform = new(_pixels, (int)world.Player.XOffset, (int)world.Player.YOffset);
            DrawRenderedWorld(worldScreenTransform);
            worldInteractionRenderer.DrawBuildings(spriteBatch, world, worldViewportService, worldScreenTransform);
            DrawPlayerShip();
            debugHudRenderer.Draw(spriteBatch, world);
            gameplayHudRenderer.Draw(spriteBatch, world, inventoryService, logicalViewportWidth);
            DrawInteractionPrompt();
            uiManager.Draw(spriteBatch, world, logicalViewportWidth, logicalViewportHeight);
            gadgetBarRenderer.Draw(spriteBatch, world, logicalViewportWidth, logicalViewportHeight, uiMousePosition, inventoryOverlay);
            inventoryOverlay?.DrawHeldItemOnTop(spriteBatch);
            deathOverlay.Draw(spriteBatch, logicalViewportWidth, playerDeathSystem.ShouldShowDeathMessage);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(sceneRenderTarget, GetPresentationRectangle(), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawRenderedWorld(UiWorld.WorldScreenTransform worldScreenTransform)
        {
            foreach (KeyValuePair<Vector2, Vector2> pair in world.WorldRender)
            {
                Rectangle destinationRectangle = worldScreenTransform.GetTileRectangle(pair.Key);
                WorldTile worldTile = new((long)pair.Value.X, (long)pair.Value.Y);

                if (world.WorldTrails.ContainsKey(pair.Value))
                {
                    spriteBatch.Draw(blocks[GameIds.RuntimeBlocks.Background].Texture, destinationRectangle, Color.White);
                }
                else
                {
                    KeyValuePair<int, (string Name, Texture2D Texture, int Frames, Block block)> block = worldBlockDefinitionResolver.GetWorldBlock(pair.Value.X, pair.Value.Y);

                    TextureAnimationHelper.Draw(spriteBatch, block.Value.Texture, destinationRectangle, block.Value.Frames, Color.White);

                    if (miningInteractions.TryGet(worldTile, out MiningInteraction interaction))
                    {
                        float percentDamaged = interaction.Block.PercentDamaged();
                        spriteBatch.Draw(blocks[GameIds.RuntimeBlocks.Breaking].Texture, destinationRectangle, Color.White * percentDamaged);
                    }
                }

                foreach (WorldEffect effect in worldEffects.GetAll(worldTile))
                {
                    (Texture2D Texture, int SpriteFrames) definition = worldEffectDefinitions[effect.Type];
                    Rectangle? sourceRectangle = TextureAnimationHelper.GetSourceRectangleForFrame(effect.PlayedFrames, definition.SpriteFrames, definition.Texture);
                    spriteBatch.Draw(definition.Texture, destinationRectangle, sourceRectangle, Color.White);
                }
            }
        }

        private void DrawPlayerShip()
        {
            playerShipRenderer.Draw(spriteBatch, world, logicalViewportWidth, logicalViewportHeight);
        }

        private void HandleClientSizeChanged(object sender, EventArgs e)
        {
            if (isApplyingResize || world == null)
            {
                return;
            }

            int width = Window.ClientBounds.Width;
            int height = Window.ClientBounds.Height;

            if (width <= 0 || height <= 0)
            {
                return;
            }

            if (graphics.PreferredBackBufferWidth != width || graphics.PreferredBackBufferHeight != height)
            {
                isApplyingResize = true;
                graphics.PreferredBackBufferWidth = width;
                graphics.PreferredBackBufferHeight = height;
                graphics.ApplyChanges();
                isApplyingResize = false;
            }
        }

        private static void NormalizeVisibleTileCounts(ref int blocksWide, ref int blocksHigh)
        {
            blocksWide = Math.Max(4, blocksWide - (blocksWide % 2 + 2));
            blocksHigh = Math.Max(4, blocksHigh - (blocksHigh % 2 + 2));
        }

        private Rectangle GetPresentationRectangle()
        {
            int viewportWidth = GraphicsDevice.Viewport.Width;
            int viewportHeight = GraphicsDevice.Viewport.Height;
            float scale = Math.Min((float)viewportWidth / logicalViewportWidth, (float)viewportHeight / logicalViewportHeight);
            int width = (int)(logicalViewportWidth * scale);
            int height = (int)(logicalViewportHeight * scale);
            int x = (viewportWidth - width) / 2;
            int y = (viewportHeight - height) / 2;
            return new Rectangle(x, y, width, height);
        }

        private MouseState CreateScaledMouseState(MouseState mouseState)
        {
            Rectangle presentationRectangle = GetPresentationRectangle();

            if (!presentationRectangle.Contains(mouseState.Position))
            {
                return new MouseState(-1, -1, mouseState.ScrollWheelValue, mouseState.LeftButton, mouseState.MiddleButton, mouseState.RightButton, mouseState.XButton1, mouseState.XButton2);
            }

            int scaledX = (int)((mouseState.X - presentationRectangle.X) * ((float)logicalViewportWidth / presentationRectangle.Width));
            int scaledY = (int)((mouseState.Y - presentationRectangle.Y) * ((float)logicalViewportHeight / presentationRectangle.Height));

            return new MouseState(scaledX, scaledY, mouseState.ScrollWheelValue, mouseState.LeftButton, mouseState.MiddleButton, mouseState.RightButton, mouseState.XButton1, mouseState.XButton2);
        }

        private void DrawInteractionPrompt()
        {
            if (uiManager.BlocksGameplay || !worldInteractionService.TryGetCurrentBuilding(world, out ABuilding building))
            {
                return;
            }
            worldInteractionRenderer.DrawInteractionPrompt(spriteBatch, building, logicalViewportHeight);
        }

        private void UpdateUiCursor(Point mousePosition)
        {
            bool shouldUseHandCursor = uiManager.IsPointerOverInteractiveElement(world, mousePosition, logicalViewportWidth, logicalViewportHeight);

            if (shouldUseHandCursor == isUsingHandCursor)
            {
                return;
            }

            Mouse.SetCursor(shouldUseHandCursor ? MouseCursor.Hand : MouseCursor.Arrow);

            isUsingHandCursor = shouldUseHandCursor;
        }
    }
}
