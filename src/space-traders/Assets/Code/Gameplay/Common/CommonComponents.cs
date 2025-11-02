using Assets.Code.Serialization;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.Gameplay.Common
{
    [Game, Meta] public class Id : ISerializeComponent { [PrimaryEntityIndex] public ulong Value; }
    [Game] public class EntityLink : IComponent { [EntityIndex] public ulong Value; }

    [Game] public class TransformComponent : IComponent { public Transform Value; }
    [Game] public class RigidbodyComponent : IComponent { public Rigidbody Value; }
    [Game] public class Active : ISerializeComponent { }

    [Game] public class CurrentScene : ISerializeComponent { public string Value; }
    [Game] public class GlobalPosition : ISerializeComponent { public double2 Value; }
    [Game] public class LocalPosition : IComponent { public Vector3 Value; }
    [Game] public class QuadrantIndex : IComponent { public int2 Value; }
}