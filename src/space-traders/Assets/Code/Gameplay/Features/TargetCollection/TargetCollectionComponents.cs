using Entitas;
using System.Collections.Generic;


namespace Assets.Code.Gameplay.Features.TargetCollection
{
    [Game] public class ReadyToCollectTargets : IComponent { }
    [Game] public class TargetsBuffer : IComponent { public List<int> Value; }
    [Game] public class CollectTargetsInterval : IComponent { public float Value; }
    [Game] public class CollectTargetsTimer : IComponent { public float Value; }
    [Game] public class Radius : IComponent { public float Value; }
}
