using Entitas;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Systems
{
    internal sealed class PlayerPreviousFramePositionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _player;

        public PlayerPreviousFramePositionSystem(GameContext game)
        {
            _player = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.ClientPlayer,
                GameMatcher.LocalPosition));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _player)
            {
                player.ReplacePreviousFrameLocalPosition(player.LocalPosition);
            }
        }
    }
}
