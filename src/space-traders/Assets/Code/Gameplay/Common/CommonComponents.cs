using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;


namespace Assets.Code.Gameplay.Common
{
    [Game, Meta] public class Id : IComponent { [PrimaryEntityIndex] public int Value; }
    [Game] public class EntityLink : IComponent { [EntityIndex] public int Value; }
    [Game] public class TransformComponent : IComponent { public Transform Value; }
    [Game] public class WorldPosition : IComponent { public Vector3 Value; }
    [Game] public class RigidbodyComponent : IComponent { public Rigidbody Value; }
    [Game] public class Active : IComponent { }
}