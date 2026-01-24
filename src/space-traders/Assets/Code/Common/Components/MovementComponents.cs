using Assets.Code.Common.Serialization;
using Entitas;
using UnityEngine;


namespace Assets.Code.Common.Components
{
    [Game] public class Velocity : IComponent { public Vector2 Value; }
    [Game] public class VelocityAgility : ISerializeComponent { public float Value; }

    [Game, Input] public class CurrentSpeedModifier : ISerializeComponent { public float Value; }
    [Game] public class MaxMoveSpeed : ISerializeComponent { public float Value; }
    [Game] public class MovingAcceleration : ISerializeComponent { public float Value; }
    [Game] public class CurrentMoveSpeed : IComponent { public float Value; }
    [Game] public class Moving : ISerializeComponent { }
    [Game] public class Braking : ISerializeComponent { }

    [Game, Input] public class TargetRotation : ISerializeComponent { public float Value; }
    [Game] public class CurrentRotationY : IComponent { public float Value; }
    [Game] public class RotationSpeed : ISerializeComponent { public float Value; }

    [Game] public class ChaseTarget : ISerializeComponent { public GameEntity Value; }
    [Game] public class MovementTarget : ISerializeComponent { public GameEntity Value; }
    [Game] public class OrbitingRadius : ISerializeComponent { public float Value; }
    [Game] public class KeepDistanceMinMax : ISerializeComponent { public Vector2 Value; }
}
