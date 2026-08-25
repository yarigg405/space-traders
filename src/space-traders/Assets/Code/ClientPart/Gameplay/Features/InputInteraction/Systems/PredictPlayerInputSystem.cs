using Entitas;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class PredictPlayerInputSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;

        public PredictPlayerInputSystem(GameContext game)
        {
            _players = game.GetGroup(GameMatcher.ClientPlayer);
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _players)
            {
                PlayerInputIntegration.Apply(player);
            }
        }
    }
}
