
using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Triggers
{
    public sealed class TriggersInteractionsService
    {
        private readonly Dictionary<uint, TriggerState> _states = new();
        private readonly List<TriggerEvent> _events = new();

        public void UpdateTrigger(uint triggerId, uint triggeredEntityId, bool isInsideNow)
        {
            if (!_states.TryGetValue(triggerId, out var state))
            {
                state = new TriggerState();
                _states[triggerId] = state;
            }

            bool wasInside = state.EntitiesInside.Contains(triggeredEntityId);

            if (isInsideNow && !wasInside)
            {
                state.EntitiesInside.Add(triggeredEntityId);
                _events.Add(new TriggerEvent(
                    triggerId, triggeredEntityId, TriggerEventType.Enter));
            }
            else if (!isInsideNow && wasInside)
            {
                state.EntitiesInside.Remove(triggeredEntityId);
                _events.Add(new TriggerEvent(
                    triggerId, triggeredEntityId, TriggerEventType.Exit));
            }
            else if (isInsideNow && wasInside)
            {
                _events.Add(new TriggerEvent(
                    triggerId, triggeredEntityId, TriggerEventType.Stay));
            }
        }

        public IReadOnlyList<TriggerEvent> ConsumeEvents()
        {
            var result = _events;
            _events.Clear();
            return result;
        }
    }
}
