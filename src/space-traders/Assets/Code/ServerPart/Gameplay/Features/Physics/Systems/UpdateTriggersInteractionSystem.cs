using Assets.Code.ServerPart.Gameplay.Features.Physics.Triggers;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class UpdateTriggersInteractionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _triggers;
        private readonly TriggersInteractionsService _interactionsService;

        public UpdateTriggersInteractionSystem(GameContext game, TriggersInteractionsService interactionsService)
        {
            _triggers = game.GetGroup(GameMatcher.CollisionsBuffer);
            _interactionsService = interactionsService;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var trigger in _triggers)
            {
                _interactionsService.UpdateInteractions(trigger.Id, trigger.CollisionsBuffer);
            }
        }
    }
}
