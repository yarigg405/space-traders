using Assets.Code.Common.Serialization;
using Entitas;
using UnityEngine;


namespace Assets.Code.Common.Components
{
    [Game] public sealed class Velocity : IComponent { public Vector2 Value; }
    [Game] public sealed class VelocityAgility : ISerializeComponent { public float Value; }

    [Game, Input] public sealed class CurrentSpeedModifier : ISerializeComponent { public float Value; }
    [Game] public sealed class MaxMoveSpeed : ISerializeComponent { public float Value; }
    [Game] public sealed class MovingAcceleration : ISerializeComponent { public float Value; }
    [Game] public sealed class CurrentMoveSpeed : IComponent { public float Value; }
    [Game] public sealed class Moving : ISerializeComponent { }

    [Game, Input] public sealed class TargetRotation : ISerializeComponent { public float Value; }
    [Game] public sealed class CurrentRotationY : IComponent { public float Value; }
    [Game] public sealed class RotationSpeed : ISerializeComponent { public float Value; }

    [Game] public sealed class ChaseTargetId : ISerializeComponent { public uint Value; }
    [Game] public sealed class MovementTargetId : ISerializeComponent { public uint Value; }
    [Game] public sealed class OrbitingRadius : ISerializeComponent { public float Value; }
    [Game] public sealed class KeepDistanceMinMax : ISerializeComponent { public Vector2 Value; }
}
