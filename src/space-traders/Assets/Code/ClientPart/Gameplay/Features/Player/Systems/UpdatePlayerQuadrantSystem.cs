using Assets.Code.ClientPart.Visual.Player;
using Entitas;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Systems
{
    internal sealed class UpdatePlayerQuadrantSystem : IExecuteSystem
    {
        private readonly PlayerQuadrantChangeObserver _playerQuadrantChangeObserver;
        private readonly IGroup<GameEntity> _player;

        public UpdatePlayerQuadrantSystem(PlayerQuadrantChangeObserver playerQuadrantChangeObserver, GameContext game)
        {
            _playerQuadrantChangeObserver = playerQuadrantChangeObserver;
            _player = game.GetGroup(GameMatcher.ClientPlayer);
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _player)
            {
                _playerQuadrantChangeObserver.ManualUpdatePlayerQuadrant(player.QuadrantIndex);
            }
        }
    }
}
