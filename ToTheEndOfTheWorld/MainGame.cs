using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
using ToTheEndOfTheWorld.Gameplay.Effects;
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
        // dotnet publish -c Release -r win-x64 --self-contained true

        private static readonly string GameTitle = "To The End Of The World";
        private static readonly string GameVersion = "V2.20";
        private static readonly int _pixels = 64;
        private const double AutosaveIntervalSeconds = 120.0;

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D pixelTexture;
        private readonly ContentManager audioContent;
        private MiningInteractionsRepository miningInteractions;
        private WorldEffectsRepository worldEffects;
        private WorldEffectDefinitionsRepository worldEffectDefinitions;
        private ScreenEffectsRepository screenEffects;
        private ScreenEffectDefinitionsRepository screenEffectDefinitions;
        private WorldElementsRepository blocks;
        private GameItemsRepository items;
        private ModelWorld world;
        private readonly PlayerInputMapper inputMapper = new();
        private readonly PlayerFacingResolver playerFacingResolver = new();
        private readonly PlayerMovementSystem playerMovementSystem = new();
        private readonly PlayerFuelSystem playerFuelSystem = new();
        private readonly PlayerHeatSystem playerHeatSystem = new();
        private readonly PlayerHullSystem playerHullSystem;
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
        private readonly UiWorld.ScreenEffectRenderer screenEffectRenderer = new();
        private ScreenEffectService screenEffectService;
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
        private MainMenuOverlay mainMenuOverlay;
        private WorldBootstrapper worldBootstrapper;
        private KeyboardState previousKeyboardState;
        private MouseState previousMouseState;
        private bool isApplyingResize;
        private bool isUsingHandCursor;
        private bool isPlayerGrounded;
        private double autosaveElapsedSeconds;
        private Task autoSaveTask = Task.CompletedTask;
        private bool isWindowFocused = true;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            playerHullSystem = new PlayerHullSystem(eventBus);
            playerVerticalImpactService = new PlayerVerticalImpactService(playerHullSystem);
            worldInteractionService = new WorldInteractionService();
            shopService = new ShopService(eventBus);
            Content.RootDirectory = "Content/Graphics";
            audioContent = new ContentManager(Services, "Content");
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += HandleClientSizeChanged;
            Activated += (_, _) => isWindowFocused = true;
            Deactivated += (_, _) => isWindowFocused = false;
            Exiting += HandleExiting;
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
            StorageChestBuildingFactory storageChestBuildingFactory = new();
            worldBootstrapper = new(sellShopBuildingFactory, equipmentShopBuildingFactory, fuelStationBuildingFactory, gadgetShopBuildingFactory, storageChestBuildingFactory);
            worldBlockDefinitionResolver = new WorldBlockDefinitionResolver(blocks);
            worldBlockFactory = new WorldBlockFactory(worldBlockDefinitionResolver);
            CraftingService craftingService = new(eventBus, new CraftingRecipeLibrary(items).CreateRecipes());
            _ = new WorldBlockLootSystem(eventBus, new BlockLootResolver(blocks), inventoryService);
            EquipmentShopService equipmentShopService = new(inventoryItemUseService, inventoryService, items, eventBus);
            uiManager = UiComposition.Create(inventoryService, craftingService, inventoryItemUseService, shopService, equipmentShopService, fuelStationService, gadgetShopService, blocks, items);
            inventoryOverlay = uiManager.GetOverlay<InventoryOverlay>();
            playerDeathSystem = new PlayerDeathSystem(items, worldViewportService, eventBus);
            playerShipRenderer = new UiWorld.PlayerShipRenderer(items, _pixels);
            gadgetBarRenderer = new UiWorld.GadgetBarRenderer(blocks, items);
            gameplayAudioSystem = new GameplayAudioSystem(audioService, eventBus);

            (int windowW, int windowH, int _blocksWide, int _blocksHigh) = CalculateInitialWindowSize();

            Window.Title = $"{GameTitle} {GameVersion}";
            graphics.PreferredBackBufferWidth = windowW;
            graphics.PreferredBackBufferHeight = windowH;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            miningInteractions = new MiningInteractionsRepository();
            worldEffects = new WorldEffectsRepository();
            screenEffects = new ScreenEffectsRepository();
            WorldBlockDamageService worldBlockDamageService = new(worldBlockDefinitionResolver, worldBlockFactory, miningInteractions, eventBus);
            DynamiteConsumeableService dynamiteConsumeableService = new(worldBlockDamageService, worldEffects, eventBus);
            FuelCapsuleConsumeableService fuelCapsuleConsumeableService = new(eventBus);
            CoolantPatchConsumeableService coolantPatchConsumeableService = new(eventBus);
            HullRepairKitConsumeableService hullRepairKitConsumeableService = new(eventBus);
            playerWorldMovementResolver = new PlayerWorldMovementResolver(worldBlockDefinitionResolver, worldViewportService, playerVerticalImpactService, _pixels);
            playerMiningSystem = new PlayerMiningSystem(worldBlockDefinitionResolver, worldBlockDamageService, playerHeatSystem, playerHullSystem, playerFuelSystem, playerVerticalImpactService, _pixels);
            playerConsumeableSystem = new PlayerConsumeableSystem(dynamiteConsumeableService, fuelCapsuleConsumeableService, coolantPatchConsumeableService, hullRepairKitConsumeableService);

            world = ContextHandler.LoadWorld();

            world ??= CreateNewWorld(_blocksWide, _blocksHigh);
            world.BlocksWide = _blocksWide;
            world.BlocksHigh = _blocksHigh;
            Vector2 initialWorldPosition = world.SavedPlayerWorldPosition != Vector2.Zero
                ? world.SavedPlayerWorldPosition
                : world.SpawnWorldPosition;
            worldViewportService.EnsurePadding(world, initialWorldPosition);
            world.SavedPlayerWorldPosition = worldViewportService.GetCenterWorldPosition(world);
            worldBootstrapper.EnsureInitialized(world);
            inventoryItemUseService.TryAlignGadgetSlotsWithInventory(world);
            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();

            base.Initialize();
        }

        private ModelWorld CreateNewWorld(int _blocksWide, int _blocksHigh)
        {
            // Fixed spawn position - Y=10 is just above ground level
            const float spawnWorldX = 0f;
            const float spawnWorldY = 10f;
            
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
                Coordinates = new Vector2(spawnWorldX, spawnWorldY),
                Cash = 10000000000f // Starting allowance
            };

            ModelWorld world = new(
                Player: player,
                Buildings: [],
                BlocksWide: _blocksWide,
                BlocksHigh: _blocksHigh,
                WorldRender: [],
                WorldTrails: []
            ) { SpawnWorldPosition = new Vector2(spawnWorldX, spawnWorldY) };

            return world;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            pixelTexture.SetData([Color.White]);
            Texture2D youDiedTexture = Content.Load<Texture2D>("General/YouDiedText");
            deathOverlay = new UiWorld.DeathOverlay(youDiedTexture);
            mainMenuOverlay = new MainMenuOverlay(SaveWorld, SelfDestruct, ResetWorld, ToggleFullscreen, Exit);
            mainMenuOverlay.LoadContent(GraphicsDevice, Content);
            debugHudRenderer.LoadContent(Content);
            gameplayHudRenderer.LoadContent(GraphicsDevice, Content);
            gadgetBarRenderer.LoadContent(GraphicsDevice, Content);
            worldInteractionRenderer.LoadContent(GraphicsDevice, Content);
            worldEffectDefinitions = new WorldEffectDefinitionsRepository(Content);
            screenEffectDefinitions = new ScreenEffectDefinitionsRepository(Content);
            screenEffectService = new ScreenEffectService(screenEffects, screenEffectDefinitions, eventBus);
            playerShipRenderer.LoadContent(Content);
            uiManager.LoadContent(GraphicsDevice, Content);
            audioService.LoadContent(audioContent);
        }

        protected override void Update(GameTime gameTime)
        {
            double totalSeconds = gameTime.TotalGameTime.TotalSeconds;
            TextureAnimationHelper.TotalSeconds = totalSeconds;
            int windowWidth = GraphicsDevice.Viewport.Width;
            int windowHeight = GraphicsDevice.Viewport.Height;
            KeyboardState keyboardState = isWindowFocused ? Keyboard.GetState() : default;
            MouseState mouseState = isWindowFocused ? Mouse.GetState() : default;

            HandleUiInput(keyboardState, windowWidth, windowHeight);

            uiManager.Update(gameTime, keyboardState, previousKeyboardState, mouseState, previousMouseState, world, windowWidth, windowHeight);
            mainMenuOverlay.Update(gameTime, keyboardState, previousKeyboardState, mouseState, previousMouseState, world, windowWidth, windowHeight);

            UpdateUiCursor(mouseState.Position, windowWidth, windowHeight);
            worldEffects.Update();
            screenEffects.Update();
            UpdateAutoSave(gameTime);

            if (inventoryOverlay?.ConsumeTrashSoundRequest() == true)
            {
                eventBus.Publish(new TrashBinUsedEvent());
            }

            if (inventoryOverlay?.ConsumeSelectionSoundRequest() == true)
            {
                audioService.PlayOneShot(SoundEffectId.EffectSelectedItem);
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

            if (uiManager.BlocksGameplay || mainMenuOverlay.BlocksGameplay)
            {
                gameplayAudioSystem.StopAllLoops();
                previousKeyboardState = keyboardState;
                previousMouseState = mouseState;
                base.Update(gameTime);

                return;
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
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
            isPlayerGrounded = isGrounded;

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

        private void HandleUiInput(KeyboardState keyboardState, int windowWidth, int windowHeight)
        {
            if (UiInputHelper.WasJustPressed(keyboardState, previousKeyboardState, Keys.E))
            {
                if (uiManager.HasOpenInteractionOverlay)
                {
                    uiManager.CloseTopmost(world);
                    return;
                }

                if (inventoryOverlay?.IsOpen == true)
                {
                    inventoryOverlay.Close(world);
                }

                worldInteractionService.TryHandleInteraction(uiManager, world, windowWidth, windowHeight);
                return;
            }

            if (UiInputHelper.WasJustPressed(keyboardState, previousKeyboardState, Keys.I))
            {
                if (uiManager.HasOpenInteractionOverlay)
                {
                    uiManager.CloseTopmost(world);
                    inventoryOverlay?.Open(windowWidth, windowHeight, world.Player);
                    return;
                }

                if (inventoryOverlay?.IsOpen == true)
                {
                    inventoryOverlay.Close(world);
                    return;
                }

                inventoryOverlay?.Open(windowWidth, windowHeight, world.Player);
                return;
            }

            if (UiInputHelper.WasJustPressed(keyboardState, previousKeyboardState, Keys.Escape))
            {
                if (!TryCloseTopmostOverlay())
                {
                    mainMenuOverlay.Open();
                }
            }
        }

        private bool TryCloseTopmostOverlay()
        {
            if (mainMenuOverlay.IsOpen)
            {
                mainMenuOverlay.Close(world);
                return true;
            }

            if (uiManager.HasOpenInteractionOverlay)
            {
                uiManager.CloseTopmost(world);
                return true;
            }

            if (inventoryOverlay?.IsOpen == true)
            {
                inventoryOverlay.Close(world);
                return true;
            }

            return false;
        }

        protected override void Draw(GameTime gameTime)
        {
            int windowWidth = GraphicsDevice.Viewport.Width;
            int windowHeight = GraphicsDevice.Viewport.Height;
            bool showOverlayBackground = inventoryOverlay?.IsOpen == true || uiManager.BlocksGameplay || mainMenuOverlay.IsOpen;

            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // World (1:1)
            UiWorld.WorldScreenTransform worldScreenTransform = new(_pixels, (int)world.Player.XOffset, (int)world.Player.YOffset, windowWidth, windowHeight, world.BlocksWide, world.BlocksHigh);
            DrawRenderedWorld(worldScreenTransform, windowWidth, windowHeight);
            worldInteractionRenderer.DrawBuildings(spriteBatch, world, worldViewportService, worldScreenTransform);
            DrawPlayerShip(windowWidth, windowHeight);

            // Screen dim
            if (showOverlayBackground)
                UiDrawHelper.DrawScreenDim(spriteBatch, pixelTexture, windowWidth, windowHeight);

            // UI (1:1)
            gadgetBarRenderer.Draw(spriteBatch, world, windowWidth, windowHeight, Mouse.GetState().Position, inventoryOverlay);
            uiManager.Draw(spriteBatch, world, windowWidth, windowHeight);
            inventoryOverlay?.DrawHeldItemOnTop(spriteBatch);
            mainMenuOverlay.Draw(spriteBatch, world, windowWidth, windowHeight);

            // HUD
            screenEffectRenderer.Draw(spriteBatch, screenEffects, screenEffectDefinitions, windowWidth, windowHeight);
            debugHudRenderer.Draw(spriteBatch, world);
            gameplayHudRenderer.Draw(spriteBatch, world, inventoryService, windowWidth);
            DrawInteractionPrompt(windowHeight);
            deathOverlay.Draw(spriteBatch, windowWidth, playerDeathSystem.ShouldShowDeathMessage);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawRenderedWorld(UiWorld.WorldScreenTransform worldScreenTransform, int windowWidth, int windowHeight)
        {
            foreach (KeyValuePair<Vector2, Vector2> pair in world.WorldRender)
            {
                Rectangle destinationRectangle = worldScreenTransform.GetTileRectangle(pair.Key);

                // Skip tiles completely outside the window
                if (destinationRectangle.Right < 0 || destinationRectangle.Left > windowWidth ||
                    destinationRectangle.Bottom < 0 || destinationRectangle.Top > windowHeight)
                    continue;

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

        private void DrawPlayerShip(int windowWidth, int windowHeight)
        {
            playerShipRenderer.Draw(spriteBatch, world, windowWidth, windowHeight, isPlayerGrounded);
        }

        private void HandleClientSizeChanged(object sender, EventArgs e)
        {
            if (isApplyingResize || world == null) return;

            int width = Window.ClientBounds.Width;
            int height = Window.ClientBounds.Height;
            if (width <= 0 || height <= 0) return;

            isApplyingResize = true;

            int blocksWide = ((width + _pixels - 1) / _pixels) | 1;
            int blocksHigh = ((height + _pixels - 1) / _pixels) | 1;
            blocksWide += 2;
            blocksHigh += 2;

            if (blocksWide != world.BlocksWide || blocksHigh != world.BlocksHigh)
            {
                Vector2 centerWorldPosition = worldViewportService.GetCenterWorldPosition(world);
                world.BlocksWide = blocksWide;
                world.BlocksHigh = blocksHigh;
                worldViewportService.EnsurePadding(world, centerWorldPosition);
            }

            isApplyingResize = false;
        }

        private void HandleExiting(object sender, EventArgs e)
        {
            if (world == null)
            {
                return;
            }

            if (!autoSaveTask.IsCompleted)
            {
                autoSaveTask.GetAwaiter().GetResult();
                return;
            }

            ContextHandler.SaveWorld(world);
        }

        private void SaveWorld()
        {
            if (world == null) return;

            ContextHandler.SaveWorld(world);
        }

        private void SelfDestruct()
        {
            playerVerticalImpactService.Clear();
            playerDeathSystem.SelfDestruct(world);
        }

        private void ResetWorld()
        {
            miningInteractions.Clear();
            worldEffects.Clear();
            screenEffects.Clear();
            world = CreateNewWorld(world.BlocksWide, world.BlocksHigh);
            worldViewportService.EnsurePadding(world, world.SpawnWorldPosition);
            world.SavedPlayerWorldPosition = world.SpawnWorldPosition;
            worldBootstrapper.EnsureInitialized(world);
            playerVerticalImpactService.Clear();
        }

        private void ToggleFullscreen()
        {
            graphics.HardwareModeSwitch = false;
            graphics.ToggleFullScreen();
        }

        private (int windowWidth, int windowHeight, int blocksWide, int blocksHigh) CalculateInitialWindowSize()
        {
            int w = (GraphicsDevice.DisplayMode.Width - 128) / _pixels | 1;
            int h = (GraphicsDevice.DisplayMode.Height - 128) / _pixels | 1;
            return (w * _pixels, h * _pixels, w + 2, h + 2);
        }

        private void DrawInteractionPrompt(int viewportHeight)
        {
            if (uiManager.BlocksGameplay || !worldInteractionService.TryGetCurrentBuilding(world, out ABuilding building))
                return;

            worldInteractionRenderer.DrawInteractionPrompt(spriteBatch, building, viewportHeight);
        }

        private void UpdateUiCursor(Point mousePosition, int windowWidth, int windowHeight)
        {
            bool shouldUseHandCursor = uiManager.IsPointerOverInteractiveElement(world, mousePosition, windowWidth, windowHeight);
            if (shouldUseHandCursor == isUsingHandCursor) return;

            Mouse.SetCursor(shouldUseHandCursor ? MouseCursor.Hand : MouseCursor.Arrow);
            isUsingHandCursor = shouldUseHandCursor;
        }

        private void UpdateAutoSave(GameTime gameTime)
        {
            if (world == null)
            {
                return;
            }

            autosaveElapsedSeconds += gameTime.ElapsedGameTime.TotalSeconds;

            if (autosaveElapsedSeconds < AutosaveIntervalSeconds)
            {
                return;
            }

            autosaveElapsedSeconds -= AutosaveIntervalSeconds;

            if (!autoSaveTask.IsCompleted)
            {
                return;
            }

            autoSaveTask = ContextHandler.SaveWorldAsync(world);
        }
    }
}