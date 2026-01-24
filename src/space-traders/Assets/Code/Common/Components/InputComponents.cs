using Entitas;
using UnityEngine;


namespace Assets.Code.Common.Components
{
    [Input] public sealed class Input : IComponent { }
    [Input] public sealed class InputPlayerTarget : IComponent { public ushort Value; }
    [Input] public sealed class ClickedPosition : IComponent { public Vector3 Value; }
}
