using Entitas;


namespace Assets.Code.Gameplay.Features.Player
{
    [Game] public class Player : IComponent { }
    [Game] public class PlayerNetworkId : IComponent { ushort Value; }
}
