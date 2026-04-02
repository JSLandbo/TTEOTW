using Microsoft.Xna.Framework.Input;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI;
using ToTheEndOfTheWorld.UI.Common;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class WorldInteractionService
    {
        public bool TryHandleInteraction(KeyboardState currentState, KeyboardState previousState, UiManager uiManager, ModelWorld world)
        {
            if (!UiInputHelper.WasJustPressed(currentState, previousState, Keys.E))
            {
                return false;
            }

            if (!TryGetCurrentBuilding(world, out ABuilding building) || building.Interaction == EBuildingInteraction.None)
            {
                return false;
            }

            return uiManager.Open(building);
        }

        public bool TryGetCurrentBuilding(ModelWorld world, out ABuilding building)
        {
            Microsoft.Xna.Framework.Vector2 worldPosition = PlayerWorldPositionService.GetPlayerWorldPosition(world);
            long tileX = (long)worldPosition.X;
            long tileY = (long)worldPosition.Y;

            foreach (ABuilding candidate in world.Buildings)
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
    }
}
