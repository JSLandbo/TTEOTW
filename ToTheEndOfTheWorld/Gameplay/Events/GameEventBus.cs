using System;
using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Gameplay.Events
{
    public sealed class GameEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> handlersByType = new();

        public void Subscribe<TEvent>(Action<TEvent> handler)
        {
            var eventType = typeof(TEvent);

            if (!handlersByType.TryGetValue(eventType, out var handlers))
            {
                handlers = new List<Delegate>();
                handlersByType.Add(eventType, handlers);
            }

            handlers.Add(handler);
        }

        public void Publish<TEvent>(TEvent gameEvent)
        {
            if (!handlersByType.TryGetValue(typeof(TEvent), out var handlers))
            {
                return;
            }

            foreach (var handler in handlers)
            {
                ((Action<TEvent>)handler)(gameEvent);
            }
        }
    }
}
