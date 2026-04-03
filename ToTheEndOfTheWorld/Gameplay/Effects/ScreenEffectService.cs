using ToTheEndOfTheWorld.Gameplay.Events;

namespace ToTheEndOfTheWorld.Gameplay.Effects
{
    public sealed class ScreenEffectService
    {
        private readonly ScreenEffectsRepository screenEffects;
        private readonly ScreenEffectDefinitionsRepository definitions;

        public ScreenEffectService(ScreenEffectsRepository screenEffects, ScreenEffectDefinitionsRepository definitions, GameEventBus eventBus)
        {
            this.screenEffects = screenEffects;
            this.definitions = definitions;
            eventBus.Subscribe<ScreenEffectRequestedEvent>(OnScreenEffectRequested);
        }

        private void OnScreenEffectRequested(ScreenEffectRequestedEvent gameEvent)
        {
            ScreenEffectDefinition definition = definitions[gameEvent.Type].Definition;
            screenEffects.Add(new ScreenEffect(gameEvent.Type, definition.PlaybackFrames, definition.Size));
        }
    }
}
