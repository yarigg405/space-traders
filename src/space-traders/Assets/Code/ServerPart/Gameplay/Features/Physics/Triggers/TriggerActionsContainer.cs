using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Triggers
{
    public sealed class TriggerActionsContainer
    {
        public List<uint> CollisionsBuffer = new(4);

        public List<uint> TriggerEnterEventHandler = new(4);
        public List<uint> TriggerExitEventHandler = new(4);
        public List<uint> TriggerStayEventHandler = new(4);
    }
}
