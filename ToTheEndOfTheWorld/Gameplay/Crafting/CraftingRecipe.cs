using System;
using ModelLibrary.Abstract.Types;

namespace ToTheEndOfTheWorld.Gameplay.Crafting
{
    public sealed record CraftingRecipe(CraftingIngredient[,] Pattern, Func<AType> CreateOutput, int OutputCount);
}
