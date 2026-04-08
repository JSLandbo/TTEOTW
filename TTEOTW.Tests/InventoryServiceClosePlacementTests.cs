using Microsoft.Xna.Framework;
using ModelLibrary.Abstract.Grids;
using ModelLibrary.Abstract.PlayerShipComponents;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Concrete;
using ModelLibrary.Concrete.Grids;
using ModelLibrary.Concrete.Items;
using ModelLibrary.Concrete.PlayerShipComponents;
using ToTheEndOfTheWorld.Gameplay;
using Xunit;

namespace TTEOTW.Tests;

/*
When InventoryService places a source slot during inventory close,
then the call site decides which inventory is primary, secondary, and tertiary.

The placement order is always:
1. Try primary.
2. Group primary.
3. Try primary again.
4. Try secondary.
5. Group secondary.
6. Try secondary again.
7. Try tertiary.
8. Group tertiary.
9. Try tertiary again.
10. Delete any final remainder.

"Try" is never all-or-nothing.
If a destination can accept one or more items, it must accept that partial amount,
and only the real remainder may continue to the next step.
*/
public sealed class InventoryServiceClosePlacementTests
{
    private const int ChestMaxStackSize = 512;

    [Fact]
    public void PrimarySource_PlacesInPrimaryFirst()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: true, inventoryColumns: 3);
        SmallDynamite target = CreateConsumable(1, "Target");
        TestChest chest = new(3);
        GridBox slot = new(target, 5);

        service.PlaceSlotDuringClose(player.Inventory, chest.Inventory, player.GadgetSlots, slot);

        Assert.Equal(5, CountInInventory(player.Inventory, target));
        Assert.Equal(0, chest.Count(target));
        Assert.Equal(0, CountInInventory(player.GadgetSlots, target));
    }

    [Fact]
    public void PrimarySource_UsesGroupedPrimaryBeforeSecondary()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: false, inventoryColumns: 3);
        SmallDynamite target = CreateConsumable(1, "Target");
        Item filler = CreateItem(50, "Filler");
        TestChest chest = new(3);
        GridBox slot = new(target, 2);

        Fill(player.Inventory.Items.InternalGrid[0, 0], target, 63);
        Fill(player.Inventory.Items.InternalGrid[1, 0], filler, 10);
        Fill(player.Inventory.Items.InternalGrid[2, 0], filler, 10);

        service.PlaceSlotDuringClose(player.Inventory, chest.Inventory, null, slot);

        Assert.Equal(65, CountInInventory(player.Inventory, target));
        Assert.Equal(0, chest.Count(target));
    }

    [Fact]
    public void PrimarySource_UsesSecondaryAfterPrimaryPaths()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: true, inventoryColumns: 1);
        SmallDynamite target = CreateConsumable(1, "Target");
        TestChest chest = new(3);
        GridBox slot = new(target, 5);

        Fill(player.Inventory.Items.InternalGrid[0, 0], CreateItem(70, "Inventory Blocker"), 1);
        FillAllUtilitySlots(player, 80);

        service.PlaceSlotDuringClose(player.Inventory, chest.Inventory, player.GadgetSlots, slot);

        Assert.Equal(5, chest.Count(target));
        Assert.Equal(0, CountInInventory(player.GadgetSlots, target));
    }

    [Fact]
    public void PrimarySource_UsesGroupedSecondaryBeforeTertiary()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: true, inventoryColumns: 1);
        SmallDynamite target = CreateConsumable(1, "Target");
        Item chestFiller = CreateItem(90, "Chest Filler");
        TestChest chest = new(3);
        GridBox slot = new(target, 2);

        Fill(player.Inventory.Items.InternalGrid[0, 0], CreateItem(91, "Inventory Blocker"), 1);
        FillAllUtilitySlots(player, 92);
        Fill(chest.Inventory.Items.InternalGrid[0, 0], target, 511);
        Fill(chest.Inventory.Items.InternalGrid[1, 0], chestFiller, 10);
        Fill(chest.Inventory.Items.InternalGrid[2, 0], chestFiller, 10);

        service.PlaceSlotDuringClose(player.Inventory, chest.Inventory, player.GadgetSlots, slot);

        Assert.Equal(513, chest.Count(target));
        Assert.Equal(0, CountInInventory(player.GadgetSlots, target));
    }

    [Fact]
    public void PrimarySource_UsesTertiaryAfterSecondaryPaths()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: true, inventoryColumns: 1);
        SmallDynamite target = CreateConsumable(1, "Target");
        TestChest chest = new(3);
        GridBox slot = new(target, 5);

        Fill(player.Inventory.Items.InternalGrid[0, 0], CreateItem(100, "Inventory Blocker"), 1);
        FillChestCompletely(chest, 101);

        service.PlaceSlotDuringClose(player.Inventory, chest.Inventory, player.GadgetSlots, slot);

        Assert.Equal(5, CountInInventory(player.GadgetSlots, target));
        Assert.Equal(0, chest.Count(target));
    }

    [Fact]
    public void PrimarySource_UsesGroupedTertiaryBeforeDeleting()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: true, inventoryColumns: 1);
        SmallDynamite target = CreateConsumable(1, "Target");
        SmallDynamite gadgetFiller = CreateConsumable(2, "Gadget Filler");
        SmallDynamite otherGadgetFiller = CreateConsumable(3, "Other Gadget Filler");
        TestChest chest = new(3);
        GridBox slot = new(target, 2);

        Fill(player.Inventory.Items.InternalGrid[0, 0], CreateItem(110, "Inventory Blocker"), 1);
        FillChestCompletely(chest, 111);
        Fill(player.GadgetSlots.Items.InternalGrid[0, 0], target, 63);
        Fill(player.GadgetSlots.Items.InternalGrid[1, 0], gadgetFiller, 10);
        Fill(player.GadgetSlots.Items.InternalGrid[2, 0], gadgetFiller, 10);
        Fill(player.GadgetSlots.Items.InternalGrid[3, 0], otherGadgetFiller, 1);

        service.PlaceSlotDuringClose(player.Inventory, chest.Inventory, player.GadgetSlots, slot);

        Assert.Equal(65, CountInInventory(player.GadgetSlots, target));
    }

    [Fact]
    public void PrimarySource_DeletesFinalRemainderWhenNothingCanAcceptIt()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: true, inventoryColumns: 1);
        SmallDynamite target = CreateConsumable(1, "Target");
        TestChest chest = new(3);
        GridBox slot = new(target, 5);

        Fill(player.Inventory.Items.InternalGrid[0, 0], CreateItem(120, "Inventory Blocker"), 1);
        FillAllUtilitySlots(player, 121);
        FillChestCompletely(chest, 122);

        service.PlaceSlotDuringClose(player.Inventory, chest.Inventory, player.GadgetSlots, slot);

        Assert.Equal(0, CountInInventory(player.Inventory, target));
        Assert.Equal(0, chest.Count(target));
        Assert.Equal(0, CountInInventory(player.GadgetSlots, target));
    }

    [Fact]
    public void SecondarySource_PlacesInPrimaryFirst()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: true, inventoryColumns: 3);
        SmallDynamite target = CreateConsumable(1, "Target");
        TestChest chest = new(3);
        GridBox slot = new(target, 5);

        service.PlaceSlotDuringClose(chest.Inventory, player.Inventory, player.GadgetSlots, slot);

        Assert.Equal(5, chest.Count(target));
        Assert.Equal(0, CountInInventory(player.Inventory, target));
        Assert.Equal(0, CountInInventory(player.GadgetSlots, target));
    }

    [Fact]
    public void SecondarySource_UsesGroupedPrimaryBeforeSecondary()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: true, inventoryColumns: 3);
        SmallDynamite target = CreateConsumable(1, "Target");
        Item chestFiller = CreateItem(130, "Chest Filler");
        TestChest chest = new(3);
        GridBox slot = new(target, 2);

        Fill(chest.Inventory.Items.InternalGrid[0, 0], target, 511);
        Fill(chest.Inventory.Items.InternalGrid[1, 0], chestFiller, 10);
        Fill(chest.Inventory.Items.InternalGrid[2, 0], chestFiller, 10);

        service.PlaceSlotDuringClose(chest.Inventory, player.Inventory, player.GadgetSlots, slot);

        Assert.Equal(513, chest.Count(target));
        Assert.Equal(0, CountInInventory(player.Inventory, target));
    }

    [Fact]
    public void SecondarySource_UsesSecondaryAfterPrimaryPaths()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: true, inventoryColumns: 3);
        SmallDynamite target = CreateConsumable(1, "Target");
        TestChest chest = new(3);
        GridBox slot = new(target, 5);

        FillChestCompletely(chest, 140);

        service.PlaceSlotDuringClose(chest.Inventory, player.Inventory, player.GadgetSlots, slot);

        Assert.Equal(5, CountInInventory(player.Inventory, target));
        Assert.Equal(0, CountInInventory(player.GadgetSlots, target));
    }

    [Fact]
    public void SecondarySource_UsesGroupedSecondaryBeforeTertiary()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: true, inventoryColumns: 3);
        SmallDynamite target = CreateConsumable(1, "Target");
        Item inventoryFiller = CreateItem(150, "Inventory Filler");
        TestChest chest = new(3);
        GridBox slot = new(target, 2);

        FillChestCompletely(chest, 151);
        Fill(player.Inventory.Items.InternalGrid[0, 0], target, 63);
        Fill(player.Inventory.Items.InternalGrid[1, 0], inventoryFiller, 10);
        Fill(player.Inventory.Items.InternalGrid[2, 0], inventoryFiller, 10);
        FillAllUtilitySlots(player, 152);

        service.PlaceSlotDuringClose(chest.Inventory, player.Inventory, player.GadgetSlots, slot);

        Assert.Equal(65, CountInInventory(player.Inventory, target));
        Assert.Equal(0, CountInInventory(player.GadgetSlots, target));
    }

    [Fact]
    public void SecondarySource_UsesTertiaryAfterSecondaryPaths()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: true, inventoryColumns: 1);
        SmallDynamite target = CreateConsumable(1, "Target");
        TestChest chest = new(3);
        GridBox slot = new(target, 5);

        FillChestCompletely(chest, 160);
        Fill(player.Inventory.Items.InternalGrid[0, 0], CreateItem(161, "Inventory Blocker"), 1);

        service.PlaceSlotDuringClose(chest.Inventory, player.Inventory, player.GadgetSlots, slot);

        Assert.Equal(5, CountInInventory(player.GadgetSlots, target));
    }

    [Fact]
    public void SecondarySource_UsesGroupedTertiaryBeforeDeleting()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: true, inventoryColumns: 1);
        SmallDynamite target = CreateConsumable(1, "Target");
        SmallDynamite gadgetFiller = CreateConsumable(2, "Gadget Filler");
        SmallDynamite otherGadgetFiller = CreateConsumable(3, "Other Gadget Filler");
        TestChest chest = new(3);
        GridBox slot = new(target, 2);

        FillChestCompletely(chest, 170);
        Fill(player.Inventory.Items.InternalGrid[0, 0], CreateItem(171, "Inventory Blocker"), 1);
        Fill(player.GadgetSlots.Items.InternalGrid[0, 0], target, 63);
        Fill(player.GadgetSlots.Items.InternalGrid[1, 0], gadgetFiller, 10);
        Fill(player.GadgetSlots.Items.InternalGrid[2, 0], gadgetFiller, 10);
        Fill(player.GadgetSlots.Items.InternalGrid[3, 0], otherGadgetFiller, 1);

        service.PlaceSlotDuringClose(chest.Inventory, player.Inventory, player.GadgetSlots, slot);

        Assert.Equal(65, CountInInventory(player.GadgetSlots, target));
    }

    [Fact]
    public void SecondarySource_DeletesFinalRemainderWhenNothingCanAcceptIt()
    {
        InventoryService service = new();
        Player player = CreatePlayer(hasGadgetBelt: true, inventoryColumns: 1);
        SmallDynamite target = CreateConsumable(1, "Target");
        TestChest chest = new(3);
        GridBox slot = new(target, 5);

        FillChestCompletely(chest, 180);
        Fill(player.Inventory.Items.InternalGrid[0, 0], CreateItem(181, "Inventory Blocker"), 1);
        FillAllUtilitySlots(player, 182);

        service.PlaceSlotDuringClose(chest.Inventory, player.Inventory, player.GadgetSlots, slot);

        Assert.Equal(0, chest.Count(target));
        Assert.Equal(0, CountInInventory(player.Inventory, target));
        Assert.Equal(0, CountInInventory(player.GadgetSlots, target));
    }

    private static Player CreatePlayer(bool hasGadgetBelt, int inventoryColumns)
    {
        Inventory inventory = new(
            ID: 1,
            Items: CreateGrid(inventoryColumns, 1),
            Name: "Inventory",
            Worth: 0,
            Weight: 0,
            MaxStackSize: 64);

        GadgetInventory gadgetSlots = new(
            ID: 2,
            Items: CreateGrid(6, 1),
            Name: "Gadget Slots",
            Worth: 0,
            Weight: 0,
            MaxStackSize: 64);

        return new Player(
            Engine: new Engine(1, 1, 1, "Engine", 0, 0, 0, 0),
            Hull: new Hull(2, 1, 100, "Hull", 0, 0),
            Drill: new Drill(3, 1, 1, "Drill"),
            Inventory: inventory,
            Thruster: new Thruster(4, 1, 1, 1, "Thruster", 0, 0, 0, 0),
            FuelTank: new FuelTank(5, 100, 100, "Fuel", 0, 0),
            ThermalPlating: new ThermalPlating(6, 100, 100, 1, "Thermal", 0, 0),
            GadgetSlots: gadgetSlots,
            HasGadgetBelt: hasGadgetBelt);
    }

    private static Grid CreateGrid(int columns, int rows)
    {
        return new Grid(Vector2.Zero, new GridBox[columns, rows]);
    }

    private static SmallDynamite CreateConsumable(short id, string name)
    {
        return new SmallDynamite(id, name, ExplosionAreaSize: 1, ExplosionPlaybackFrames: 1, Damage: 1, MaxHardness: 1);
    }

    private static Item CreateItem(short id, string name)
    {
        return new Item(id, name);
    }

    private static void Fill(AGridBox slot, AType item, int count)
    {
        slot.Item = item;
        slot.Count = count;
    }

    private static void FillAllUtilitySlots(Player player, short startingId)
    {
        for (int x = 0; x < 4; x++)
        {
            Fill(player.GadgetSlots.Items.InternalGrid[x, 0], CreateConsumable((short)(startingId + x), $"Utility {x}"), 64);
        }
    }

    private static void FillChestCompletely(TestChest chest, short startingId)
    {
        Fill(chest.Inventory.Items.InternalGrid[0, 0], CreateItem(startingId, "Secondary A"), ChestMaxStackSize);
        Fill(chest.Inventory.Items.InternalGrid[1, 0], CreateItem((short)(startingId + 1), "Secondary B"), ChestMaxStackSize);
        Fill(chest.Inventory.Items.InternalGrid[2, 0], CreateItem((short)(startingId + 2), "Secondary C"), ChestMaxStackSize);
    }

    private static int CountInInventory(AInventory inventory, AType item)
    {
        int total = 0;
        foreach (AGridBox slot in InventoryService.EnumerateSlots(inventory.Items.InternalGrid))
        {
            if (InventoryService.CanStackTogether(slot.Item, item))
            {
                total += slot.Count;
            }
        }

        return total;
    }

    private sealed class TestChest(int columns)
    {
        public Inventory Inventory { get; } = new(
            ID: 99,
            Items: CreateGrid(columns, 1),
            Name: "Secondary",
            Worth: 0,
            Weight: 0,
            MaxStackSize: ChestMaxStackSize);

        public int Count(AType item)
        {
            return CountInInventory(Inventory, item);
        }
    }
}
