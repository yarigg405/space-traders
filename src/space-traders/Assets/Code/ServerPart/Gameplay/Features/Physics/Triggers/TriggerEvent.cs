namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Triggers
{
    public readonly struct TriggerEvent
    {
        public readonly uint TriggerId;
        public readonly uint TriggeredEntityId;
        public readonly TriggerEventType EventType;

        public TriggerEvent(uint triggerId, uint triggeredEntityId, TriggerEventType eventType)
        {
            EventType = eventType;
            TriggeredEntityId = triggeredEntityId;
            TriggerId = triggerId;
        }
    }

    public enum TriggerEventType
    {
        Enter,
        Stay,
        Exit,
    }

}
