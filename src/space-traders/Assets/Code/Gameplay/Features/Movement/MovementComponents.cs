using Entitas;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.Movement
{
    [Game] public class DirectionalMovement : IComponent { public Vector2 Value; }
    [Game] public class CurrentRotationY : IComponent { public float Value; }
    [Game] public class Moving : IComponent { }

}
