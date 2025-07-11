using Assets.Code.Gameplay.Common;
using Assets.Code.Gameplay.Features.Player.Factory;
using Entitas;


namespace Assets.Code.Gameplay.Features.Player.Systems
{
    internal sealed class InitializeHeroSystem : IInitializeSystem
    {
        private readonly PlayerFactory _playerFactory;

        private readonly Vector2Double _playerStartPos = new Vector2Double(0, 0);

        internal InitializeHeroSystem(GameContext game, PlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        void IInitializeSystem.Initialize()
        {
            _playerFactory.CreatePlayer(_playerStartPos);
        }
    }
}