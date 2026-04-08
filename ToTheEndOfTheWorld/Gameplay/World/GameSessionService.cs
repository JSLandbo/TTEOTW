using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ModelLibrary.Abstract;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Concrete.PlayerShipComponents;
using ModelPlayer = ModelLibrary.Concrete.Player;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class GameSessionService(
        GameItemsRepository items,
        WorldBootstrapper worldBootstrapper,
        WorldViewportService worldViewportService,
        MiningInteractionsRepository miningInteractions,
        WorldEffectsRepository worldEffects,
        ScreenEffectsRepository screenEffects,
        PlayerVerticalImpactService playerVerticalImpactService,
        PlayerDeathSystem playerDeathSystem,
        Func<int, int, ModelWorld> createNewWorld)
    {
        public void SelfDestruct(ModelWorld world)
        {
            playerVerticalImpactService.Clear();
            playerDeathSystem.SelfDestruct(world);
        }

        public ModelWorld ResetWorld(ModelWorld world)
        {
            ClearTransientState();

            APlayer previousPlayer = world.Player;
            List<ABuilding> preservedBuildings = world.Buildings;
            ModelWorld resetWorld = createNewWorld(world.BlocksWide, world.BlocksHigh);
            resetWorld.Player = CreateResetWorldPlayer(previousPlayer, resetWorld.SpawnWorldPosition);
            resetWorld.Buildings = preservedBuildings;

            FinalizeWorld(resetWorld);
            return resetWorld;
        }

        public ModelWorld ResetGame(ModelWorld world)
        {
            ClearTransientState();

            ModelWorld resetWorld = createNewWorld(world.BlocksWide, world.BlocksHigh);
            FinalizeWorld(resetWorld);
            return resetWorld;
        }

        private void ClearTransientState()
        {
            miningInteractions.Clear();
            worldEffects.Clear();
            screenEffects.Clear();
            playerVerticalImpactService.Clear();
        }

        private void FinalizeWorld(ModelWorld world)
        {
            worldViewportService.EnsurePadding(world, world.SpawnWorldPosition);
            world.SavedPlayerWorldPosition = world.SpawnWorldPosition;
            worldBootstrapper.EnsureInitialized(world);
        }

        private ModelPlayer CreateResetWorldPlayer(APlayer previousPlayer, Vector2 spawnWorldPosition)
        {
            Inventory inventory = new((Inventory)previousPlayer.Inventory);
            GadgetInventory gadgetSlots = new((GadgetInventory)previousPlayer.GadgetSlots);
            ClearGrid(inventory.Items.InternalGrid);
            ClearGrid(gadgetSlots.Items.InternalGrid);

            return new ModelPlayer(
                ThermalPlating: items.Create<ThermalPlating>(previousPlayer.ThermalPlating.ID),
                Engine: items.Create<Engine>(previousPlayer.Engine.ID),
                Hull: items.Create<Hull>(previousPlayer.Hull.ID),
                Drill: items.Create<Drill>(previousPlayer.Drill.ID),
                Inventory: inventory,
                Thruster: items.Create<Thruster>(previousPlayer.Thruster.ID),
                FuelTank: items.Create<FuelTank>(previousPlayer.FuelTank.ID),
                GadgetSlots: gadgetSlots,
                HasGadgetBelt: previousPlayer.HasGadgetBelt)
            {
                Coordinates = spawnWorldPosition,
                Cash = previousPlayer.Cash
            };
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
    }
}
