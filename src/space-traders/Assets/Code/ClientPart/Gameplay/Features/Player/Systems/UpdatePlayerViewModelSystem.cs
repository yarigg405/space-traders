using Assets.Code.ClientPart.Visual.Player;
using Entitas;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Systems
{
    internal sealed class UpdatePlayerViewModelSystem : IExecuteSystem
    {
        private readonly PlayerViewModel _playerViewModel;
        private readonly IGroup<GameEntity> _player;

        public UpdatePlayerViewModelSystem(PlayerViewModel viewModel, GameContext game)
        {
            _playerViewModel = viewModel;
            _player = game.GetGroup(GameMatcher.ClientPlayer);
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _player)
            {
                _playerViewModel.ManualUpdatePlayerModel(player);
            }
        }
    }
}
