using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Triggers
{
    public sealed class TriggersInteractionsService
    {
        private readonly Dictionary<uint, HashSet<uint>> _previous = new();
        private readonly List<TriggerEvent> _events = new();


        public void UpdateInteractions(uint triggerId, List<uint> currentEntities)
        {
            if (!_previous.ContainsKey(triggerId))
            {
                _previous[triggerId] = new();
            }

            foreach (var entityId in currentEntities)
            {
                if (_previous[triggerId].Contains(entityId))
                {
                    _events.Add(new TriggerEvent(
                        triggerId,
                        entityId,
                        TriggerEventType.Stay));
                }
                else
                {
                    _previous[triggerId].Add(entityId);
                    _events.Add(new TriggerEvent(
                        triggerId,
                        entityId,
                        TriggerEventType.Enter));
                }
            }

            foreach (var entityId in _previous[triggerId].ToArray())
            {
                if (!currentEntities.Contains(entityId))
                {
                    _previous[triggerId].Remove(entityId);
                    _events.Add(new TriggerEvent(
                        triggerId,
                        entityId,
                        TriggerEventType.Exit));
                }
            }
        }

        public void RemoveTrigger(uint triggerId)
        {
            UpdateInteractions(triggerId, new());
            _previous.Remove(triggerId);
        }

        public void RemoveEntity(uint entityId)
        {
            foreach (var kvp in _previous)
            {
                uint triggerId = kvp.Key;
                var insideSet = kvp.Value;

                if (insideSet.Remove(entityId))
                {
                    _events.Add(new TriggerEvent(
                        triggerId,
                        entityId,
                        TriggerEventType.Exit));
                }
            }
        }

        public IEnumerable<TriggerEvent> ConsumeEvents()
        {
            var result = _events.ToList();
            _events.Clear();
            return result;
        }
    }
}
