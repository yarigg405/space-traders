using Assets.Code.ServerPart.Gameplay.Features.Physics.Triggers;
using Entitas;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class TriggerEventHandlersSystem : IExecuteSystem
    {
        private readonly TriggersInteractionsService _triggersInteractionsService;

        public TriggerEventHandlersSystem(TriggersInteractionsService service)
        {
            _triggersInteractionsService = service;
        }

        void IExecuteSystem.Execute()
        {
            var events = _triggersInteractionsService.ConsumeEvents();
            foreach (var ev in events)
            {
                Debug.Log($"{ev.TriggeredEntityId} - {ev.EventType} - {ev.TriggerId}");
            }
        }
    }
}
