using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Context
{
    public sealed class MiningInteractionsRepository
    {
        private readonly Dictionary<WorldTile, MiningInteraction> interactionsByTile = [];

        public bool TryGet(WorldTile worldTile, out MiningInteraction interaction)
        {
            return interactionsByTile.TryGetValue(worldTile, out interaction);
        }

        public void Add(MiningInteraction interaction)
        {
            for (long x = interaction.TileBounds.X; x < interaction.TileBounds.X + interaction.TileBounds.Width; x++)
            {
                for (long y = interaction.TileBounds.Y; y < interaction.TileBounds.Y + interaction.TileBounds.Height; y++)
                {
                    WorldTile worldTile = new(x, y);
                    interactionsByTile[worldTile] = interaction;
                }
            }
        }

        public void Remove(MiningInteraction interaction)
        {
            for (long x = interaction.TileBounds.X; x < interaction.TileBounds.X + interaction.TileBounds.Width; x++)
            {
                for (long y = interaction.TileBounds.Y; y < interaction.TileBounds.Y + interaction.TileBounds.Height; y++)
                {
                    WorldTile worldTile = new(x, y);
                    if (interactionsByTile.TryGetValue(worldTile, out MiningInteraction existingInteraction) && ReferenceEquals(existingInteraction, interaction))
                    {
                        interactionsByTile.Remove(worldTile);
                    }
                }
            }
        }
    }
}
