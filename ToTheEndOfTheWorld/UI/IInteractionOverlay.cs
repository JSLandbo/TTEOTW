using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.UI
{
    public interface IInteractionOverlay : IGameOverlay
    {
        EBuildingInteraction Action { get; }
        void Open(ABuilding building);
    }
}
