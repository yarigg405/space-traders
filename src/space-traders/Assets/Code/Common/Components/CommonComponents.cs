using Assets.Code.ClientPart.View;
using Assets.Code.Common.Serialization;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.Common.Components
{
    [Game, Meta] public sealed class Id : ISerializeComponent { [PrimaryEntityIndex] public uint Value; }
    [Game] public sealed class EntityLink : IComponent { [EntityIndex] public uint Value; }
    [Game] public sealed class ParentEntity : IComponent { public uint Value; }
    [Game] public sealed class ChildrenEntities : IComponent { public List<GameEntity> Value; }

    [Game] public sealed class TransformComponent : IComponent { public Transform Value; }
    [Game] public sealed class RigidbodyComponent : IComponent { public Rigidbody Value; }
    [Game] public sealed class Active : ISerializeComponent { }

    [Game] public sealed class View : IComponent { public EntityBehaviour Value; }
    [Game] public sealed class ViewPath : ISerializeComponent { public string Value; }
    [Game] public sealed class ViewPrefab : IComponent { public EntityBehaviour Value; }
    [Game] public sealed class Destructed : ISerializeComponent { }
    [Game] public sealed class Disposed : IComponent { }
    [Game] public sealed class SelfDestructTimer : ISerializeComponent { public float Value; }
    [Game] public sealed class NeedSynchronize : IComponent { }
    [Game] public sealed class ViewModelComponent : IComponent { public ViewModel Value; }

    [Game] public sealed class GlobalPosition : ISerializeComponent { public double2 Value; }
    [Game] public sealed class LocalPosition : IComponent { public Vector3 Value; }
    [Game] public sealed class QuadrantIndex : ISerializeComponent { public int2 Value; }

    [Game] public sealed class DatabaseId : ISerializeComponent { public int Value; }
    [Game] public sealed class DatabaseName : ISerializeComponent { public string Value; }
}