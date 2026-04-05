using ModelLibrary.Abstract.Buildings;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.UI
{
    public interface IInteractionOverlay : IGameOverlay
    {
        EBuildingInteraction Action { get; }
        int PanelWidth { get; }
        void Open(ABuilding building, int viewportWidth, int viewportHeight);
        void SetPanelOffset(int offsetX);
    }
}
