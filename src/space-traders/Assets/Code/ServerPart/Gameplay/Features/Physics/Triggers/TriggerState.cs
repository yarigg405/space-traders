using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Triggers
{
    public sealed class TriggerState
    {
        public HashSet<uint> EntitiesInside = new();
    }
}
