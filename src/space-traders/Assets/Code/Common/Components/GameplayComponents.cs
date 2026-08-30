using Assets.Code.Common.DataContainers;
using Assets.Code.Common.Serialization;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.Common.Components
{
    [Game] public sealed class ShipsInDockZone : IComponent { public List<GameEntity> Value; }
    [Game] public sealed class ShipCanBeDocked : ISerializeComponent { public DockingDataContainer Value; }
    [Game] public sealed class DockingInProcess : ISerializeComponent { }
    [Game] public sealed class UndockingInProcess : ISerializeComponent { }
    [Game] public sealed class UndockingTimer : IComponent { public float Value; }
    [Game] public sealed class AnimatedMovingDataContainerComponent : IComponent { public AnimatedMoveDataContainer Value; }
}
