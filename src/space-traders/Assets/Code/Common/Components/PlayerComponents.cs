using Assets.Code.Common.Serialization;
using Entitas;

namespace Assets.Code.Common.Components
{
    [Game] public sealed class Player : ISerializeComponent { }
    [Game] public sealed class ClientPlayer : IComponent { }
    [Game] public sealed class PlayerNetworkId : ISerializeComponent { public ushort Value; }
}
