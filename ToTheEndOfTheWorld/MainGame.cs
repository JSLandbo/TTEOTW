using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete;
using ModelLibrary.Concrete.PlayerShipComponents;
using System;
using System.Collections.Generic;
using ToTheEndOfTheWorld.Context;
using ToTheEndOfTheWorld.Context.StaticRepositories;
using ToTheEndOfTheWorld.Gameplay;
using ToTheEndOfTheWorld.Gameplay.Events;
using ToTheEndOfTheWorld.UI;
using ToTheEndOfTheWorld.UI.WorldRendering;

namespace ToTheEndOfTheWorld
{
    public class MainGame : Game
    {
        private static readonly string GameTitle = "To The End Of The World";
        private static readonly string GameVersion = "V1.10";
        private static readonly int _pixels = 64;

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D sceneRenderTarget;
        private WorldInteractionsRepository interactions;
        private WorldElementsRepository blocks;
        private GameItemsRepository items;
        private World world;
        private readonly PlayerInputMapper inputMapper = new();
        private readonly PlayerFacingResolver playerFacingResolver = new();
        private readonly PlayerMovementSystem playerMovementSystem = new();
        private WorldBootstrapper worldBootstrapper;
        private readonly WorldInteractionService worldInteractionService;
        private readonly WorldViewportService worldViewportService = new();
        private readonly GameEventBus eventBus = new();
        private readonly InventoryService inventoryService = new();
        private readonly ShopService shopService = new();
        private InventoryItemUseService inventoryItemUseService;
        private EquipmentShopService equipmentShopService;
        private EquipmentShopBuildingFactory equipmentShopBuildingFactory;
        private readonly DebugHudRenderer debugHudRenderer = new();
        private readonly GameplayHudRenderer gameplayHudRenderer = new();
        private readonly WorldInteractionRenderer worldInteractionRenderer = new();
        private WorldBlockDefinitionResolver worldBlockDefinitionResolver;
        private WorldBlockFactory worldBlockFactory;
        private PlayerWorldMovementResolver playerWorldMovementResolver;
        private PlayerMiningSystem playerMiningSystem;
        private PlayerShipRenderer playerShipRenderer;
        private CraftingService craftingService;
        private UiManager uiManager;
        private int logicalViewportWidth;
        private int logicalViewportHeight;
        private KeyboardState previousKeyboardState;
        private MouseState previousMouseState;
        private bool isApplyingResize;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            worldInteractionService = new WorldInteractionService();
            Content.RootDirectory = "Content";
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
            inventoryItemUseService = new InventoryItemUseService(inventoryService, items);
            equipmentShopBuildingFactory = new EquipmentShopBuildingFactory(items);
            worldBootstrapper = new WorldBootstrapper(worldViewportService, equipmentShopBuildingFactory);
            worldBlockDefinitionResolver = new WorldBlockDefinitionResolver(blocks);
            worldBlockFactory = new WorldBlockFactory(worldBlockDefinitionResolver);
            craftingService = new CraftingService();
            _ = new WorldBlockLootSystem(eventBus, new BlockLootResolver(blocks), inventoryService);
            equipmentShopService = new EquipmentShopService(inventoryService, items);
            uiManager = UiComposition.Create(inventoryService, craftingService, inventoryItemUseService, shopService, equipmentShopService, blocks, items);
            playerShipRenderer = new PlayerShipRenderer(items, _pixels);

            var _blocksWide = (GraphicsDevice.DisplayMode.Width - (GraphicsDevice.DisplayMode.Width % _pixels)) / _pixels;
            var _blocksHigh = (GraphicsDevice.DisplayMode.Height - (GraphicsDevice.DisplayMode.Height % _pixels)) / _pixels;

            NormalizeVisibleTileCounts(ref _blocksWide, ref _blocksHigh);

            Window.Title = $"{GameTitle} {GameVersion}";
            graphics.PreferredBackBufferWidth = _blocksWide * _pixels;
            graphics.PreferredBackBufferHeight = _blocksHigh * _pixels;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            logicalViewportWidth = graphics.PreferredBackBufferWidth;
            logicalViewportHeight = graphics.PreferredBackBufferHeight;

            interactions = new WorldInteractionsRepository();
            playerWorldMovementResolver = new PlayerWorldMovementResolver(worldBlockDefinitionResolver, worldViewportService, _pixels);
            playerMiningSystem = new PlayerMiningSystem(worldBlockDefinitionResolver, worldBlockFactory, interactions, eventBus, _pixels);

            world = ContextHandler.LoadWorld();

            world ??= CreateNewWorld(_blocksWide, _blocksHigh);
            world.BlocksWide = _blocksWide;
            world.BlocksHigh = _blocksHigh;
            worldViewportService.EnsurePadding(world);
            worldBootstrapper.EnsureInitialized(world);
            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();

            base.Initialize();
        }

        private World CreateNewWorld(int _blocksWide, int _blocksHigh)
        {
            var player = new Player(
                ThermalPlating: items.Create<ThermalPlating>(10000),
                Engine: items.Create<Engine>(10100),
                Hull: items.Create<Hull>(10500),
                Drill: items.Create<Drill>(10600),
                Inventory: items.Create<Inventory>(10300),
                Thruster: items.Create<Thruster>(10400),
                FuelTank: items.Create<FuelTank>(10200)
            )
            {
                Coordinates = new Vector2((float)Math.Floor(_blocksWide / 2.0d), (float)Math.Floor(_blocksHigh / 2.0d))
            };

            return new World(
                Player: player,                                  // ContextHandler.LoadPlayer();
                Buildings: new List<ABuilding>(),                // ContextHandler.LoadBuildings();
                BlocksWide: _blocksWide,                         // Calculated
                BlocksHigh: _blocksHigh,                         // Calculated
                WorldRender: new Dictionary<Vector2, Vector2>(), // Dynamically updated
                WorldTrails: new Dictionary<Vector2, bool>()     // ContextHandler.LoadWorldTrails();
            );
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sceneRenderTarget = new RenderTarget2D(GraphicsDevice, logicalViewportWidth, logicalViewportHeight);
            debugHudRenderer.LoadContent(Content);
            gameplayHudRenderer.LoadContent(GraphicsDevice, Content);
            worldInteractionRenderer.LoadContent(GraphicsDevice, Content);
            uiManager.LoadContent(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = CreateScaledMouseState(Mouse.GetState());
            var blockedGameplayAtFrameStart = uiManager.BlocksGameplay;
            uiManager.Update(gameTime, keyboardState, previousKeyboardState, mouseState, previousMouseState, world, logicalViewportWidth, logicalViewportHeight);

            if (uiManager.BlocksGameplay)
            {
                previousKeyboardState = keyboardState;
                previousMouseState = mouseState;
                base.Update(gameTime);
                return;
            }

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var intent = inputMapper.ReadPlayerIntent(keyboardState, previousKeyboardState);

            var player = world.Player;
            var facingDirection = playerFacingResolver.Resolve(player, intent);
            player.ApplyIntent(intent.MovementInput, facingDirection);
            playerMovementSystem.Update(player, deltaTime);

            var resolutionSteps = playerWorldMovementResolver.EstimateRequiredIterations(player);
            for (var i = 0; i < resolutionSteps; i++)
            {
                playerMiningSystem.Update(world, player);

                if (!playerWorldMovementResolver.ResolveStep(world, player))
                {
                    break;
                }
            }

            if (!blockedGameplayAtFrameStart)
            {
                worldInteractionService.TryHandleInteraction(keyboardState, previousKeyboardState, uiManager, world);
            }

            previousKeyboardState = keyboardState;
            previousMouseState = mouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(sceneRenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            DrawRenderedWorld();
            worldInteractionRenderer.DrawBuildings(spriteBatch, world, worldViewportService, _pixels);
            DrawPlayerShip();
            debugHudRenderer.Draw(spriteBatch, world);
            gameplayHudRenderer.Draw(spriteBatch, world, inventoryService, logicalViewportWidth);
            DrawInteractionPrompt();
            uiManager.Draw(spriteBatch, world, logicalViewportWidth, logicalViewportHeight);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.LinearClamp);
            spriteBatch.Draw(sceneRenderTarget, GetPresentationRectangle(), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawRenderedWorld()
        {
            foreach (var pair in world.WorldRender)
            {
                var player = world.Player;
                var location = new Vector2(
                    pair.Key.X * _pixels - (0.5f * _pixels),
                    pair.Key.Y * _pixels - (0.5f * _pixels)
                );

                location.X -= player.XOffset;
                location.Y -= player.YOffset;


                if (world.WorldTrails.ContainsKey(pair.Value))
                {
                    spriteBatch.Draw(blocks[-1].Texture, location, Color.White);
                }
                else
                {
                    var block = worldBlockDefinitionResolver.GetWorldBlock(pair.Value.X, pair.Value.Y);

                    spriteBatch.Draw(block.Value.Texture, location, Color.White);

                    if (interactions.TryGet(new WorldTile((long)pair.Value.X, (long)pair.Value.Y), WorldInteractionType.Mining, out var interaction))
                    {
                        var percentDamaged = interaction.Block.PercentDamaged();

                        spriteBatch.Draw(blocks[-2].Texture, location, Color.White * percentDamaged);
                    }
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

            var width = Window.ClientBounds.Width;
            var height = Window.ClientBounds.Height;

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
            var viewportWidth = GraphicsDevice.Viewport.Width;
            var viewportHeight = GraphicsDevice.Viewport.Height;
            var scale = Math.Min((float)viewportWidth / logicalViewportWidth, (float)viewportHeight / logicalViewportHeight);
            var width = (int)(logicalViewportWidth * scale);
            var height = (int)(logicalViewportHeight * scale);
            var x = (viewportWidth - width) / 2;
            var y = (viewportHeight - height) / 2;
            return new Rectangle(x, y, width, height);
        }

        private MouseState CreateScaledMouseState(MouseState mouseState)
        {
            var presentationRectangle = GetPresentationRectangle();

            if (!presentationRectangle.Contains(mouseState.Position))
            {
                return new MouseState(-1, -1, mouseState.ScrollWheelValue, mouseState.LeftButton, mouseState.MiddleButton, mouseState.RightButton, mouseState.XButton1, mouseState.XButton2);
            }

            var scaledX = (int)((mouseState.X - presentationRectangle.X) * ((float)logicalViewportWidth / presentationRectangle.Width));
            var scaledY = (int)((mouseState.Y - presentationRectangle.Y) * ((float)logicalViewportHeight / presentationRectangle.Height));

            return new MouseState(scaledX, scaledY, mouseState.ScrollWheelValue, mouseState.LeftButton, mouseState.MiddleButton, mouseState.RightButton, mouseState.XButton1, mouseState.XButton2);
        }

        private void DrawInteractionPrompt()
        {
            if (uiManager.BlocksGameplay || !worldInteractionService.TryGetCurrentBuilding(world, out var building))
            {
                return;
            }
            worldInteractionRenderer.DrawInteractionPrompt(spriteBatch, building, logicalViewportWidth, logicalViewportHeight);
        }

    }
}
