namespace Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerProvider : IPlayerProvider, IPlayerProviderSetupper
    {
        GameEntity IPlayerProvider.PlayerEntity => _playerEntity;
        private GameEntity _playerEntity;

        void IPlayerProviderSetupper.SetPlayer(GameEntity playerEntity)
        {
            _playerEntity = playerEntity;
        }
    }
}
