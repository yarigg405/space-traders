using Assets.Code.ClientPart.View;
using Assets.Code.Common.Serialization;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.Common.Components
{
    [Game, Meta] public class Id : ISerializeComponent { [PrimaryEntityIndex] public uint Value; }
    [Game] public class EntityLink : IComponent { [EntityIndex] public uint Value; }

    [Game] public class TransformComponent : IComponent { public Transform Value; }
    [Game] public class RigidbodyComponent : IComponent { public Rigidbody Value; }
    [Game] public class Active : ISerializeComponent { }

    [Game] public class View : IComponent { public EntityBehaviour Value; }
    [Game] public class ViewPath : ISerializeComponent { public string Value; }
    [Game] public class ViewPrefab : IComponent { public EntityBehaviour Value; }
    [Game] public class Destructed : ISerializeComponent { }
    [Game] public class SelfDestructTimer : ISerializeComponent { public float Value; }


    [Game] public class GlobalPosition : ISerializeComponent { public double2 Value; }
    [Game] public class LocalPosition : IComponent { public Vector3 Value; }
    [Game] public class QuadrantIndex : IComponent { public int2 Value; }
}