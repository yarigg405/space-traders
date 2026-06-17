using Entitas;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerProvider : IPlayerProvider, IPlayerProviderSetupper
    {
        GameEntity IPlayerProvider.PlayerEntity => _playerEntity;
        private GameEntity _playerEntity;

        void IPlayerProviderSetupper.SetPlayer(GameEntity playerEntity)
        {
            _playerEntity = playerEntity;
            _playerEntity.OnDestroyEntity += OnDestroyEntity;
        }

        private void OnDestroyEntity(IEntity entity)
        {
            _playerEntity.OnDestroyEntity -= OnDestroyEntity;
            _playerEntity = null;
        }
    }
}
