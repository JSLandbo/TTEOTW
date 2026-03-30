using System;
using System.Collections.Generic;

namespace ToTheEndOfTheWorld.Gameplay.Events
{
    public sealed class GameEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> handlersByType = [];

        public void Subscribe<TEvent>(Action<TEvent> handler)
        {
            Type eventType = typeof(TEvent);

            if (!handlersByType.TryGetValue(eventType, out List<Delegate> handlers))
            {
                handlers = [];
                handlersByType.Add(eventType, handlers);
            }

            handlers.Add(handler);
        }

        public void Publish<TEvent>(TEvent gameEvent)
        {
            if (!handlersByType.TryGetValue(typeof(TEvent), out List<Delegate> handlers))
            {
                return;
            }

            foreach (Delegate handler in handlers)
            {
                ((Action<TEvent>)handler)(gameEvent);
            }
        }
    }
}
