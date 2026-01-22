using Assets.Code.Common.Serialization;
using Entitas;

namespace Assets.Code.Common.Components
{
    [Game] public class Player : ISerializeComponent { }
    [Game] public class ClientPlayer : IComponent { }
    [Game] public class PlayerNetworkId : ISerializeComponent { public ushort Value; }
}
