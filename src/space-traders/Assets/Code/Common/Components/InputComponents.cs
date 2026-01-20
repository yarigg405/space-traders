using Entitas;
using UnityEngine;


namespace Assets.Code.Common.Components
{
    [Input] public class Input : IComponent { }
    [Input] public class ClickedPosition : IComponent { public Vector3 Value; }
}
