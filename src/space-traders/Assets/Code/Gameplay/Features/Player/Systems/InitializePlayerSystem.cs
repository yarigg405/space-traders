using Assets.Code.Gameplay.Common;
using Assets.Code.Gameplay.Features.Player.Factory;
using Entitas;
using Unity.Mathematics;


namespace Assets.Code.Gameplay.Features.Player.Systems
{
    internal sealed class InitializePlayerSystem : IInitializeSystem
    {
        private readonly PlayerFactory _playerFactory;

        internal InitializePlayerSystem(GameContext game, PlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        void IInitializeSystem.Initialize()
        {
            _playerFactory.CreatePlayer(double2.zero.GetRandomCoordinatesAroundPointZX(15));
        }
    }
}