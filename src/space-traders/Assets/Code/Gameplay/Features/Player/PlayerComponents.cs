using Assets.Code.Serialization;
using Entitas;


namespace Assets.Code.Gameplay.Features.Player
{
    [Game] public class Player : ISerializeComponent { }
    [Game] public class PlayerNetworkId : ISerializeComponent { public ushort Value; }
}
