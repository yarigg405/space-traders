using Assets.Code.Common.Serialization;

namespace Assets.Code.Common.Components
{
    [Game] public class Player : ISerializeComponent { }
    [Game] public class PlayerNetworkId : ISerializeComponent { public ushort Value; }
}
