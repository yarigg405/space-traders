using Assets.Code.ServerPart.Gameplay.Features.Movement;
using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class SetPlayerDirectionByInputSystem : IExecuteSystem
    {
        private readonly IGroup<InputEntity> _inputs;
        private readonly EntitiesSynchronizator _synchronizator;
        private readonly GameContext _game;

        public SetPlayerDirectionByInputSystem(GameContext game,
            InputContext input, EntitiesSynchronizator synchronizator)
        {
            _inputs = input.GetGroup(InputMatcher.AllOf(
                InputMatcher.Input,
                InputMatcher.TargetRotation,
                InputMatcher.InputConsumerEntityId
                ));
            _synchronizator = synchronizator;
            _game = game;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var input in _inputs)
            {
                var player = _game.GetEntityWithId(input.InputConsumerEntityId);
                player.ResetMovingComponents();

                player.ReplaceTargetRotation(input.TargetRotation);

                _synchronizator.UpdateComponentsForEntity(player,
                   MovementExtensions.GetMovementComponentsForReset());
            }
        }
    }
}
