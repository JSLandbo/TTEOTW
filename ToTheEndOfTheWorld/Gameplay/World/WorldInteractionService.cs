using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Enums;
using ToTheEndOfTheWorld.UI;

namespace ToTheEndOfTheWorld.Gameplay.World
{
    public sealed class WorldInteractionService
    {
        public bool TryHandleInteraction(UiManager uiManager, ModelWorld world, int viewportWidth, int viewportHeight)
        {
            if (!TryGetCurrentBuilding(world, out ABuilding building) || building.Interaction == EBuildingInteraction.None)
            {
                return false;
            }

            return uiManager.Open(building, world, viewportWidth, viewportHeight);
        }

        public bool TryGetCurrentBuilding(ModelWorld world, out ABuilding building)
        {
            Vector2 worldPosition = PlayerWorldPositionService.GetPlayerWorldPosition(world);
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
