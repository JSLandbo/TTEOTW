using Microsoft.Xna.Framework.Content;
using ModelLibrary.Abstract.Types;
using System.Collections.Generic;

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
            return TryGetValue(itemId, out GameItemDefinition itemDefinition) ? itemDefinition.Create() : null;
        }

        public T Create<T>(short itemId) where T : AType
        {
            return Create(itemId) as T;
        }
    }
}
