using Entitas;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.Movement
{
    [Game] public class Velocity : IComponent { public Vector2 Value; }
    [Game] public class VelocityAgility : IComponent { public float Value; }

    [Game] public class MoveSpeed : IComponent { public float Value; }
    [Game] public class Moving : IComponent { }

    [Game] public class CurrentRotationY : IComponent { public float Value; }
    [Game] public class TargetRotation : IComponent { public float Value; }
    [Game] public class RotationSpeed : IComponent { public float Value; }

    [Game] public class ChaseTarget : IComponent { public GameEntity Value; }
}
