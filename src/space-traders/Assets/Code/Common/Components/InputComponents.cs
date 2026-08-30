using Assets.Code.Common.DataContainers;
using Assets.Code.Common.Serialization;
using Entitas;
using UnityEngine;


namespace Assets.Code.Common.Components
{
    [Input] public sealed class Input : IComponent { }
    [Input] public sealed class InputConsumerEntityId : IComponent { public uint Value; }
    [Input] public sealed class PressedButtonsContainerComponent : IComponent { public PressedButtonsContainer Value; }

    [Game, Input] public sealed class MoveInput : ISerializeComponent { public Vector2 Value; }
}
