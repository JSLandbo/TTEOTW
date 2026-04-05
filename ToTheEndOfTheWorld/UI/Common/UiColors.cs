using Microsoft.Xna.Framework;

namespace ToTheEndOfTheWorld.UI.Common
{
    /// <summary>
    /// Common UI colors used across overlays and renderers.
    /// </summary>
    public static class UiColors
    {
        // Panel backgrounds
        public static readonly Color PanelBackground = new(22, 22, 22);
        public static readonly Color PanelBackgroundLight = new(24, 24, 24);
        public static readonly Color PanelShadow = new(0, 0, 0, 70);
        public static readonly Color HeaderBackground = new(44, 44, 44);
        public static readonly Color HeaderBackgroundAlt = new(42, 42, 42);
        public static readonly Color SectionBackground = new(28, 28, 28);
        public static readonly Color SectionBackgroundAlt = new(31, 31, 31);
        public static readonly Color SectionBackgroundAlt2 = new(34, 34, 34);
        public static readonly Color SectionBackgroundDark = new(48, 48, 48);
        public static readonly Color CardBackground = new(30, 30, 30);
        public static readonly Color ListBackground = new(27, 27, 27);
        public static readonly Color ListEntryBackground = new(35, 35, 35);

        // Borders
        public static readonly Color PanelBorder = new(108, 108, 108);
        public static readonly Color PanelBorderLight = new(92, 92, 92);
        public static readonly Color PanelBorderDark = new(88, 88, 88);
        public static readonly Color CardBorder = new(78, 78, 78);
        public static readonly Color ListBorder = new(68, 68, 68);
        public static readonly Color ListEntryBorder = new(52, 52, 52);
        public static readonly Color Divider = new(78, 78, 78);

        // Slot colors
        public static readonly Color SlotBackground = new(44, 44, 44);
        public static readonly Color SlotBorder = new(108, 108, 108);
        public static readonly Color SlotBackgroundInventory = new(62, 62, 62);
        public static readonly Color SlotBorderInventory = new(124, 124, 124);
        public static readonly Color SlotBackgroundEquipment = new(58, 58, 58);
        public static readonly Color SlotBorderEquipment = new(132, 132, 132);
        public static readonly Color SlotBackgroundEquipmentHighlight = new(78, 88, 78);
        public static readonly Color SlotBorderEquipmentHighlight = new(162, 194, 162);
        public static readonly Color SlotBackgroundEmpty = new(34, 34, 34);
        public static readonly Color SlotBorderEmpty = new(76, 76, 76);
        public static readonly Color SlotBackgroundDisabled = new(10, 10, 10);
        public static readonly Color SlotBorderDisabled = new(20, 20, 20);
        public static readonly Color SlotBackgroundBuyable = new(74, 62, 38);
        public static readonly Color SlotBorderBuyable = new(208, 180, 96);

        // Button colors
        public static readonly Color ButtonBackground = new(86, 86, 86);
        public static readonly Color ButtonBorder = new(146, 146, 146);
        public static readonly Color ButtonBackgroundDisabled = new(64, 64, 64);
        public static readonly Color ButtonBorderDisabled = new(110, 110, 110);

        // Sort button
        public static readonly Color SortButtonBackground = new(56, 68, 92);
        public static readonly Color SortButtonBorder = new(124, 156, 214);
        public static readonly Color SortButtonBackgroundDisabled = new(40, 44, 52);
        public static readonly Color SortButtonBorderDisabled = new(88, 96, 112);

        // Trash button
        public static readonly Color TrashButtonBackground = new(110, 58, 58);
        public static readonly Color TrashButtonBorder = new(210, 110, 110);
        public static readonly Color TrashButtonBackgroundDisabled = new(58, 40, 40);
        public static readonly Color TrashButtonBorderDisabled = new(132, 92, 92);

        // Self-destruct button
        public static readonly Color SelfDestructBackground = new(92, 36, 36);
        public static readonly Color SelfDestructBorder = new(190, 92, 92);
        public static readonly Color SelfDestructBackgroundHover = new(120, 48, 48);
        public static readonly Color SelfDestructBorderHover = new(226, 118, 118);

        // Action buttons (buy/sell/refuel)
        public static readonly Color ActionButtonBackground = new(121, 106, 77);
        public static readonly Color ActionButtonBorder = new(181, 163, 126);
        public static readonly Color ActionButtonBackgroundGreen = new(86, 110, 78);
        public static readonly Color ActionButtonBorderGreen = new(152, 182, 140);
        public static readonly Color ActionButtonBackgroundGreenAlt = new(92, 116, 82);
        public static readonly Color ActionButtonBorderGreenAlt = new(162, 196, 146);

        // Scrollbar
        public static readonly Color ScrollbarTrack = new(55, 55, 55);
        public static readonly Color ScrollbarThumb = new(170, 170, 170);

        // Text colors
        public static readonly Color TextTitle = new(244, 240, 229);
        public static readonly Color TextBody = new(230, 230, 230);
        public static readonly Color TextBodyAlt = new(214, 214, 214);
        public static readonly Color TextBodyAlt2 = new(224, 224, 224);
        public static readonly Color TextButton = new(248, 243, 233);
        public static readonly Color TextButtonAlt = new(230, 214, 166);
        public static readonly Color TextMuted = new(160, 160, 160);
        public static readonly Color TextListTitle = new(236, 236, 236);
        public static readonly Color TextListBody = new(214, 214, 214);

        // Hover label
        public static readonly Color HoverLabelBackground = new(24, 24, 24, 228);
        public static readonly Color HoverLabelBorder = new(102, 102, 102, 228);
        public static readonly Color HoverLabelText = new(244, 244, 244);

        // World interaction prompt
        public static readonly Color PromptBackground = new(0, 0, 0, 180);
    }
}
