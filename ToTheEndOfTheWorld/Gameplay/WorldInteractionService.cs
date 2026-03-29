using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Concrete;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed class WorldInteractionService
    {
        public bool TryHandleInteraction(KeyboardState currentState, KeyboardState previousState, UiManager uiManager, World world)
        {
            if (!WasJustPressed(currentState, previousState, Keys.E))
            {
                return false;
            }

            if (!TryGetCurrentBuilding(world, out var building) || building.Interaction == EBuildingInteraction.None)
            {
                return false;
            }

            return uiManager.Open(building);
        }

        public bool TryGetCurrentBuilding(World world, out ABuilding building)
        {
            if (world.Buildings == null)
            {
                building = null;
                return false;
            }

            var worldPosition = PlayerWorldPositionService.GetPlayerWorldPosition(world);
            var tileX = (long)worldPosition.X;
            var tileY = (long)worldPosition.Y;

            foreach (var candidate in world.Buildings)
            {
                if (candidate.Interaction == EBuildingInteraction.None || !candidate.ContainsTile(tileX, tileY))
                {
                    continue;
                }

                building = candidate;
                return true;
            }

            building = null;
            return false;
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
