using Assets.Code.ServerPart.Gameplay.Features.Physics.Services;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class CheckTriggersInteractionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly TriggersInteractionsService _triggersInteractions;

        public CheckTriggersInteractionSystem(TriggersInteractionsService triggersInteractions, GameContext game)
        {
            _triggersInteractions = triggersInteractions;
        }

        void IExecuteSystem.Execute()
        {
            
        }
    }
}
