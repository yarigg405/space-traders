using Assets.Code.ServerPart.Gameplay.Features.Physics.Triggers;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class CheckTriggersInteractionSystem : IExecuteSystem
    {
        private readonly TriggersInteractionsService _triggersInteractions;

        private readonly IGroup<GameEntity> _entities;
        private readonly IGroup<GameEntity> _triggerEntities;

        public CheckTriggersInteractionSystem(TriggersInteractionsService triggersInteractions, GameContext game)
        {
            _triggersInteractions = triggersInteractions;

            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.PhysicsRadius,
                GameMatcher.GlobalPosition));

            
        }

        void IExecuteSystem.Execute()
        {
            
        }
    }
}
