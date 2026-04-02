using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ModelLibrary.Abstract.Types;
using ModelLibrary.Enums;

namespace ToTheEndOfTheWorld.Context.Items
{
    public sealed class GameItemDefinition(
        string name, // TODO: What is?
        Dictionary<PlayerOrientation, Texture2D> textures,
        AType definition,
        Func<AType> create,
        bool buyable = false,
        EGameItemType type = EGameItemType.Item,
        EEquipmentType equipmentType = EEquipmentType.None,
        int tier = 0,
        int frames = 1)
    {
        public Dictionary<PlayerOrientation, Texture2D> Textures { get; } = textures;
        public AType Definition { get; } = definition;
        public bool Buyable { get; } = buyable;
        public EGameItemType Type { get; } = type;
        public EEquipmentType EquipmentType { get; } = equipmentType;
        public int Tier { get; } = tier;
        public int Frames { get; } = frames < 1 ? 1 : frames;

        public AType Create() => create();
    }
}
