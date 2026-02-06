using Assets.Code.ClientPart.View;
using Assets.Code.Common.Serialization;
using Assets.Code.ServerPart.Physics.Data;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.Common.Components
{
    [Game, Meta] public sealed class Id : ISerializeComponent { [PrimaryEntityIndex] public uint Value; }
    [Game] public sealed class EntityLink : IComponent { [EntityIndex] public uint Value; }
    [Game] public sealed class NeedInit : IComponent { }
    [Game] public sealed class ServerEntity : IComponent { }

    [Game] public sealed class TransformComponent : IComponent { public Transform Value; }
    [Game] public sealed class RigidbodyComponent : IComponent { public Rigidbody Value; }
    [Game] public sealed class Active : ISerializeComponent { }

    [Game] public sealed class View : IComponent { public EntityBehaviour Value; }
    [Game] public sealed class ViewPath : ISerializeComponent { public string Value; }
    [Game] public sealed class ViewPrefab : IComponent { public EntityBehaviour Value; }
    [Game] public sealed class Destructed : ISerializeComponent { }
    [Game] public sealed class SelfDestructTimer : ISerializeComponent { public float Value; }


    [Game] public sealed class GlobalPosition : ISerializeComponent { public double2 Value; }
    [Game] public sealed class LocalPosition : IComponent { public Vector3 Value; }
    [Game] public sealed class QuadrantIndex : ISerializeComponent { public int2 Value; }

    [Game] public sealed class PhysicsRadius : ISerializeComponent { public float Value; }
    [Game] public sealed class PhysicShape : IComponent { public PhysicsShape[] Value; }
}