using Assets.Code.Common;
using Assets.Code.ServerPart.Gameplay.Features.Movement;
using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class SetWarpMovingByInputSystem : IExecuteSystem
    {
        private readonly IGroup<InputEntity> _inputs;
        private readonly GameContext _game;
        private readonly EntitiesSynchronizator _synchronizator;

        public SetWarpMovingByInputSystem(GameContext game, InputContext input,
            EntitiesSynchronizator synchronizator)
        {
            _inputs = input.GetGroup(InputMatcher.AllOf(
                InputMatcher.Input,
                InputMatcher.WarpFinishCoordinates,
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
                if (player == null) continue;

                player.SetWarpTo(input.WarpFinishCoordinates.GetRandomCoordinatesAroundPointZX(50f));

                _synchronizator.UpdateComponentsForEntity(player,
                    MovementExtensions.GetMovementComponentsForReset());
            }
        }
    }
}
