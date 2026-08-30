using Assets.Code.Common.Serialization;
using Assets.Code.ServerPart.Physics.Data;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.Common.Components
{
    [Game] public sealed class PhysicsRadius : ISerializeComponent { public float Value; }
    [Game] public sealed class Trigger : IComponent { }
    [Game] public sealed class IgnoreCollision : ISerializeComponent { }

    [Game] public sealed class CollectCollisionsInterval : IComponent { public float Value; }
    [Game] public sealed class CollectCollisionsTimer : IComponent { public float Value; }
    [Game] public sealed class ReadyToCollectCollisions : IComponent { }

    [Game] public sealed class CollisionsBuffer : IComponent { public List<uint> Value; }
    [Game] public sealed class TriggerEnterEventHandler : IComponent { public List<uint> Value; }
    [Game] public sealed class TriggerExitEventHandler : IComponent { public List<uint> Value; }
    [Game] public sealed class TriggerStayEventHandler : IComponent { public List<uint> Value; }

    [Game] public sealed class Mass : ISerializeComponent { public float Value; }
}
