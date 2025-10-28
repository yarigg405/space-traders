using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using Unity.Mathematics;


namespace Assets.Code.Gameplay.Common
{
    [Game, Meta] public class Id : IComponent { [PrimaryEntityIndex] public ulong Value; }
    [Game] public class EntityLink : IComponent { [EntityIndex] public ulong Value; }

    [Game] public class TransformComponent : IComponent { public Transform Value; }
    [Game] public class RigidbodyComponent : IComponent { public Rigidbody Value; }
    [Game] public class Active : IComponent { }

    [Game] public class CurrentScene : IComponent { public string Value; }
    [Game] public class GlobalPosition : IComponent { public double2 Value; }
    [Game] public class LocalPosition : IComponent { public Vector3 Value; }
    [Game] public class QuadrantIndex : IComponent { public int2 Value; }
}