using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Context
{
    public sealed class WorldInteractionsRepository
    {
        private readonly Dictionary<WorldTile, List<WorldInteraction>> interactionsByTile = new();

        public bool TryGet(WorldTile worldTile, WorldInteractionType interactionType, out WorldInteraction interaction)
        {
            interaction = null;

            if (!interactionsByTile.TryGetValue(worldTile, out var interactions))
            {
                return false;
            }

            for (var i = interactions.Count - 1; i >= 0; i--)
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
            if (!interactionsByTile.TryGetValue(worldTile, out var interactions))
            {
                return new List<WorldInteraction>();
            }

            return interactions;
        }

        public IReadOnlyList<WorldInteraction> GetAll(WorldTileBounds tileBounds)
        {
            var uniqueInteractions = new HashSet<WorldInteraction>();

            for (long x = tileBounds.X; x < tileBounds.X + tileBounds.Width; x++)
            {
                for (long y = tileBounds.Y; y < tileBounds.Y + tileBounds.Height; y++)
                {
                    if (!interactionsByTile.TryGetValue(new WorldTile(x, y), out var interactions))
                    {
                        continue;
                    }

                    foreach (var interaction in interactions)
                    {
                        uniqueInteractions.Add(interaction);
                    }
                }
            }

            return new List<WorldInteraction>(uniqueInteractions);
        }

        public IReadOnlyList<WorldInteraction> GetAll()
        {
            var uniqueInteractions = new HashSet<WorldInteraction>();

            foreach (var entry in interactionsByTile)
            {
                foreach (var interaction in entry.Value)
                {
                    uniqueInteractions.Add(interaction);
                }
            }

            return new List<WorldInteraction>(uniqueInteractions);
        }

        public void Add(WorldInteraction interaction)
        {
            for (long x = interaction.TileBounds.X; x < interaction.TileBounds.X + interaction.TileBounds.Width; x++)
            {
                for (long y = interaction.TileBounds.Y; y < interaction.TileBounds.Y + interaction.TileBounds.Height; y++)
                {
                    var worldTile = new WorldTile(x, y);

                    if (!interactionsByTile.TryGetValue(worldTile, out var interactions))
                    {
                        interactions = new List<WorldInteraction>();
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
                    var worldTile = new WorldTile(x, y);

                    if (!interactionsByTile.TryGetValue(worldTile, out var interactions))
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
