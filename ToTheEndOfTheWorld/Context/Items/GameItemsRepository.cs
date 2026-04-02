using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Content;
using ModelLibrary.Abstract.Types;

namespace ToTheEndOfTheWorld.Context.Items
{
    public sealed partial class GameItemsRepository : Dictionary<int, GameItemDefinition>
    {
        public GameItemsRepository(ContentManager manager)
        {
            InitializeCollection(manager);
        }

        public AType Create(short itemId)
        {
            if (!TryGetValue(itemId, out GameItemDefinition itemDefinition))
            {
                throw new KeyNotFoundException($"No item definition registered for id {itemId}.");
            }

            return itemDefinition.Create();
        }

        public T Create<T>(short itemId) where T : AType
        {
            AType createdItem = Create(itemId);

            if (createdItem is not T typedItem)
            {
                throw new InvalidOperationException($"Item id {itemId} does not create a {typeof(T).Name}.");
            }

            return typedItem;
        }
    }
}
