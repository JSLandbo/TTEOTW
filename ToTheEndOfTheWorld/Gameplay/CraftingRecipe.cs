using ModelLibrary.Abstract.Types;
using System;

namespace ToTheEndOfTheWorld.Gameplay
{
    public sealed record CraftingRecipe(CraftingIngredient[,] Pattern, Func<AType> CreateOutput, int OutputCount);
}
