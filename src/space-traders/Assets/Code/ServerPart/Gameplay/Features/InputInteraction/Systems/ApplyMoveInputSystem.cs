using Assets.Code.ServerPart.Gameplay.Features.Movement;
using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class ApplyMoveInputSystem : IExecuteSystem
    {
        private readonly IGroup<InputEntity> _inputs;
        private readonly GameContext _game;
        private readonly EntitiesSynchronizator _synchronizator;


        public ApplyMoveInputSystem(GameContext game, InputContext input,
            EntitiesSynchronizator synchronizator)
        {
            _game = game;
            _synchronizator = synchronizator;

            _inputs = input.GetGroup(InputMatcher.AllOf(
                InputMatcher.Input,
                InputMatcher.MoveInput,
                InputMatcher.InputConsumerEntityId
                ));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var input in _inputs)
            {
                var player = _game.GetEntityWithId(input.InputConsumerEntityId);
                player.ReplaceMoveInput(input.MoveInput);

                if (input.MoveInput.sqrMagnitude > 0.0001f)
                {
                    player.ResetMovingComponents();
                    _synchronizator.UpdateComponentsForEntity(player, MovementExtensions.GetMovementComponentsForReset());
                }
            }
        }
    }
}
