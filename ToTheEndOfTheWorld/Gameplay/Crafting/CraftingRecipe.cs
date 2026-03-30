using ModelLibrary.Abstract.Types;
using System;

namespace ToTheEndOfTheWorld.Gameplay.Crafting
{
    public sealed record CraftingRecipe(CraftingIngredient[,] Pattern, Func<AType> CreateOutput, int OutputCount);
}
