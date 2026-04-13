using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.Types;

namespace ToTheEndOfTheWorld.Gameplay.Player
{
    public sealed class PlayerConsumeableSystem(
        DynamiteConsumeableService dynamiteConsumeableService,
        FuelCapsuleConsumeableService fuelCapsuleConsumeableService,
        CoolantPatchConsumeableService coolantPatchConsumeableService,
        HullRepairKitConsumeableService hullRepairKitConsumeableService,
        TeleporterConsumeableService teleporterConsumeableService)
    {
        public void TryUse(ModelWorld world, int slotIndex)
        {
            if (!world.Player.HasGadgetBelt)
            {
                return;
            }

            if (slotIndex < 0 || slotIndex >= world.Player.GadgetSlots.Items.InternalGrid.GetLength(0))
            {
                return;
            }

            AGridBox slot = world.Player.GadgetSlots.Items.InternalGrid[slotIndex, 0];

            if (slot.Item is not AConsumeable consumeable || slot.Count <= 0)
            {
                return;
            }

            if (UseConsumeable(world, consumeable))
            {
                Consume(slot);
            }
        }

        private bool UseConsumeable(ModelWorld world, AConsumeable consumeable)
        {
            if (consumeable is ADynamite dynamite)
            {
                return dynamiteConsumeableService.TryUse(world, dynamite);
            }

            if (consumeable is AFuelCapsule fuelCapsule)
            {
                return fuelCapsuleConsumeableService.TryUse(world, fuelCapsule);
            }

            if (consumeable is ACoolantPatch coolantPatch)
            {
                return coolantPatchConsumeableService.TryUse(world, coolantPatch);
            }

            if (consumeable is AHullRepairKit hullRepairKit)
            {
                return hullRepairKitConsumeableService.TryUse(world, hullRepairKit);
            }

            if (consumeable is ATeleporter teleporter)
            {
                return teleporterConsumeableService.TryUse(world, teleporter);
            }

            return false;
        }

        private static void Consume(AGridBox slot)
        {
            slot.Count -= 1;

            if (slot.Count > 0)
            {
                return;
            }

            slot.Item = null;
            slot.Count = 0;
        }
    }
}
