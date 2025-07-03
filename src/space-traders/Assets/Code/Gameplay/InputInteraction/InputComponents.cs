using Entitas;
using UnityEngine;


namespace Assets.Code.Gameplay.InputInteraction
{
    [Game] public class Input : IComponent { }
    [Game] public class ClickedPosition : IComponent { public Vector3 Value; }
}
