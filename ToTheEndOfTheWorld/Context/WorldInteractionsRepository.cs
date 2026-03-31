using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Context
{
    public sealed class WorldInteractionsRepository
    {
        private readonly Dictionary<WorldTile, List<WorldInteraction>> interactionsByTile = [];

        public bool TryGet(WorldTile worldTile, WorldInteractionType interactionType, out WorldInteraction interaction)
        {
            interaction = null;

            if (!interactionsByTile.TryGetValue(worldTile, out List<WorldInteraction> interactions))
            {
                return false;
            }

            for (int i = interactions.Count - 1; i >= 0; i--)
            {
                if (interactions[i].Type == interactionType)
                {
                    interaction = interactions[i];

                    return true;
                }
            }

            return false;
        }

        public IReadOnlyList<WorldInteraction> GetAll(WorldTile worldTile)
        {
            if (!interactionsByTile.TryGetValue(worldTile, out List<WorldInteraction> interactions))
            {
                return [];
            }

            return interactions;
        }

        public IReadOnlyList<WorldInteraction> GetAll(WorldTileBounds tileBounds)
        {
            HashSet<WorldInteraction> uniqueInteractions = [];

            for (long x = tileBounds.X; x < tileBounds.X + tileBounds.Width; x++)
            {
                for (long y = tileBounds.Y; y < tileBounds.Y + tileBounds.Height; y++)
                {
                    if (!interactionsByTile.TryGetValue(new WorldTile(x, y), out List<WorldInteraction> interactions))
                    {
                        continue;
                    }

                    foreach (WorldInteraction interaction in interactions)
                    {
                        uniqueInteractions.Add(interaction);
                    }
                }
            }

            return [.. uniqueInteractions];
        }

        public IReadOnlyList<WorldInteraction> GetAll()
        {
            HashSet<WorldInteraction> uniqueInteractions = [];

            foreach (KeyValuePair<WorldTile, List<WorldInteraction>> entry in interactionsByTile)
            {
                foreach (WorldInteraction interaction in entry.Value)
                {
                    uniqueInteractions.Add(interaction);
                }
            }

            return [.. uniqueInteractions];
        }

        public void Add(WorldInteraction interaction)
        {
            for (long x = interaction.TileBounds.X; x < interaction.TileBounds.X + interaction.TileBounds.Width; x++)
            {
                for (long y = interaction.TileBounds.Y; y < interaction.TileBounds.Y + interaction.TileBounds.Height; y++)
                {
                    WorldTile worldTile = new(x, y);

                    if (!interactionsByTile.TryGetValue(worldTile, out List<WorldInteraction> interactions))
                    {
                        interactions = [];
                        interactionsByTile.Add(worldTile, interactions);
                    }

                    interactions.Add(interaction);
                }
            }
        }

        public void Remove(WorldInteraction interaction)
        {
            for (long x = interaction.TileBounds.X; x < interaction.TileBounds.X + interaction.TileBounds.Width; x++)
            {
                for (long y = interaction.TileBounds.Y; y < interaction.TileBounds.Y + interaction.TileBounds.Height; y++)
                {
                    WorldTile worldTile = new(x, y);

                    if (!interactionsByTile.TryGetValue(worldTile, out List<WorldInteraction> interactions))
                    {
                        continue;
                    }

                    interactions.Remove(interaction);

                    if (interactions.Count == 0)
                    {
                        interactionsByTile.Remove(worldTile);
                    }
                }
            }
        }
    }
}
