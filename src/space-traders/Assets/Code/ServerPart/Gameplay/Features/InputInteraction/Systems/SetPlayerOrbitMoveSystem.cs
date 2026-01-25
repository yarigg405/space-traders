using Assets.Code.ServerPart.Gameplay.Features.Movement;
using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class SetPlayerOrbitMoveSystem : IExecuteSystem
    {
        private readonly IGroup<InputEntity> _inputs;
        private readonly GameContext _game;
        private readonly EntitiesSynchronizator _synchronizator;

        public SetPlayerOrbitMoveSystem(GameContext game, InputContext input,
            EntitiesSynchronizator synchronizator)
        {
            _inputs = input.GetGroup(InputMatcher.AllOf(
                InputMatcher.Input,
                InputMatcher.MovementTargetId,
                InputMatcher.OrbitingRadius,
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
                var target = _game.GetEntityWithId(input.MovementTargetId);
                if (target == null) continue;

                player.StartOrbitMoving(input.OrbitingRadius, target);

                _synchronizator.UpdateComponentsForEntity(player,
                    MovementExtensions.GetMovementComponentsForReset());
            }
        }
    }
}
