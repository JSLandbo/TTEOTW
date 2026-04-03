using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Concrete.PlayerShipComponents;
using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerDeathSystem(GameItemsRepository items, WorldViewportService worldViewportService, GameEventBus eventBus)
    {
        private bool awaitingRespawn;

        public bool ShouldShowDeathMessage => awaitingRespawn;

        public bool TryHandleDeath(ModelWorld world)
        {
            if (awaitingRespawn)
            {
                return true;
            }

            if (world.Player.CurrentHull > 0.0f)
            {
                return false;
            }

            EnterDeathState(world);

            return true;
        }

        public void SelfDestruct(ModelWorld world)
        {
            world.Player.CurrentHull = 0.0f;
            eventBus.Publish(new PlayerSelfDestructedEvent());
            EnterDeathState(world);
        }

        public void TryRespawnOnInput(ModelWorld world, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            if (!awaitingRespawn || !WasAnyKeyJustPressed(currentKeyboardState, previousKeyboardState))
            {
                return;
            }

            Respawn(world);
        }

        private void EnterDeathState(ModelWorld world)
        {
            awaitingRespawn = true;
            world.Player.Cash *= 0.75;
            world.Player.XVelocity = 0.0f;
            world.Player.YVelocity = 0.0f;
            world.Player.XOffset = 0.0f;
            world.Player.YOffset = 0.0f;
            world.Player.MovementInput = Vector2.Zero;
            world.Player.FacingDirection = Vector2.Zero;
            world.Player.Mining = false;
            world.Player.DrillExtended = false;
        }

        private void Respawn(ModelWorld world)
        {
            ClearInventory(world.Player.Inventory.Items.InternalGrid);
            ClearInventory(world.Player.GadgetSlots.Items.InternalGrid);

            short thermalPlatingId = world.Player.ThermalPlating.ID;
            short hullId = world.Player.Hull.ID;
            short fuelTankId = world.Player.FuelTank.ID;

            world.Player.ThermalPlating = items.Create<ThermalPlating>(thermalPlatingId);
            world.Player.Hull = items.Create<Hull>(hullId);
            world.Player.FuelTank = items.Create<FuelTank>(fuelTankId);
            world.Player.CurrentHeat = 0.0f;
            world.Player.CurrentHull = world.Player.Hull.Health;
            world.Player.CurrentFuel = world.Player.FuelTank.Capacity;

            world.Player.XVelocity = 0.0f;
            world.Player.YVelocity = 0.0f;
            world.Player.XOffset = 0.0f;
            world.Player.YOffset = 0.0f;
            world.Player.MovementInput = Vector2.Zero;
            world.Player.FacingDirection = Vector2.Zero;
            world.Player.Mining = false;
            world.Player.DrillExtended = false;

            worldViewportService.EnsurePadding(world, world.SpawnWorldPosition);

            awaitingRespawn = false;
        }

        private static void ClearInventory(AGridBox[,] grid)
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

        private static bool WasAnyKeyJustPressed(KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            Keys[] pressedKeys = currentKeyboardState.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                if (!previousKeyboardState.IsKeyDown(key))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
