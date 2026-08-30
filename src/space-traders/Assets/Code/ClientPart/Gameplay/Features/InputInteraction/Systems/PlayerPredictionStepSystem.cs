using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Entitas;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class PlayerPredictionStepSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;

        public PlayerPredictionStepSystem(GameContext game)
        {
            _players = game.GetGroup(GameMatcher.ClientPlayer);
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _players)
                if (!player.isWarping)
                    PlayerSimulationStep.Apply(player);
        }
    }
}
