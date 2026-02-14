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

        private readonly IGroup<GameEntity> _players;

        public SetWarpMovingByInputSystem(GameContext game, InputContext input,
            EntitiesSynchronizator synchronizator)
        {
            _inputs = input.GetGroup(InputMatcher.AllOf(
                InputMatcher.Input,
                InputMatcher.WarpFinishCoordinates,
                InputMatcher.InputConsumerEntityId
                ));

            _players = game.GetGroup(GameMatcher.Player);
            _synchronizator = synchronizator;
            _game = game;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var input in _inputs)
            {
                foreach (var player in _players)
                {
                    player.SetWarpTo(input.WarpFinishCoordinates.GetRandomCoordinatesAroundPointZX(50f));

                    _synchronizator.UpdateComponentsForEntity(player,
                        MovementExtensions.GetMovementComponentsForReset());
                }
            }
        }
    }
}
