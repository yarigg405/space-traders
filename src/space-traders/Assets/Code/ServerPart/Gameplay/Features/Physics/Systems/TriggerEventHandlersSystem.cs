using Assets.Code.ServerPart.Gameplay.Features.Physics.Triggers;
using Entitas;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class TriggerEventHandlersSystem : IExecuteSystem
    {
        private readonly TriggersInteractionsService _triggersInteractionsService;
        private readonly GameContext _context;

        public TriggerEventHandlersSystem(TriggersInteractionsService service,
            GameContext context)
        {
            _triggersInteractionsService = service;
            _context = context;
        }

        void IExecuteSystem.Execute()
        {
            var events = _triggersInteractionsService.ConsumeEvents();
            foreach (var ev in events)
            {
                var trigger = _context.GetEntityWithId(ev.TriggerId);

                switch (ev.EventType)
                {
                    case TriggerEventType.Enter:
                        {
                            if (trigger.hasTriggerEnterEventHandler)
                                trigger.TriggerEnterEventHandler.Add(ev.TriggeredEntityId);
                        }
                        break;

                    case TriggerEventType.Exit:
                        {
                            if (trigger.hasTriggerExitEventHandler)
                                trigger.TriggerExitEventHandler.Add(ev.TriggeredEntityId);
                        }
                        break;

                    default:
                        {
                            if (trigger.hasTriggerStayEventHandler)
                                trigger.TriggerStayEventHandler.Add(ev.TriggeredEntityId);
                        }
                        break;
                }
            }
        }
    }
}
