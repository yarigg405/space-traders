using UnityEngine;
using Entitas;


namespace Assets.Code.Common.Components
{
    [Game] public class Velocity : IComponent { public Vector2 Value; }
    [Game] public class VelocityAgility : IComponent { public float Value; }

    [Game] public class MaxMoveSpeed : IComponent { public float Value; }
    [Game] public class CurrentSpeedModifier : IComponent { public float Value; }
    [Game] public class MovingAcceleration : IComponent { public float Value; }
    [Game] public class CurrentMoveSpeed : IComponent { public float Value; }
    [Game] public class Moving : IComponent { }
    [Game] public class Braking : IComponent { }

    [Game] public class CurrentRotationY : IComponent { public float Value; }
    [Game] public class TargetRotation : IComponent { public float Value; }
    [Game] public class RotationSpeed : IComponent { public float Value; }

    [Game] public class ChaseTarget : IComponent { public GameEntity Value; }
    [Game] public class MovementTarget : IComponent { public GameEntity Value; }
    [Game] public class OrbitingRadius : IComponent { public float Value; }
    [Game] public class KeepDistanceMinMax : IComponent { public Vector2 Value; }
}
