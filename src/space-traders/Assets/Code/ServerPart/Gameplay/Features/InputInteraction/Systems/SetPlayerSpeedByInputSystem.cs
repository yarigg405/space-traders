using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class SetPlayerSpeedByInputSystem : IExecuteSystem
    {
        private readonly IGroup<InputEntity> _inputs;
        private readonly GameContext _game;
        private readonly EntitiesSynchronizator _synchronizator;

        public SetPlayerSpeedByInputSystem(GameContext game, InputContext input,
            EntitiesSynchronizator synchronizator)
        {
            _inputs = input.GetGroup(InputMatcher.AllOf(
                InputMatcher.Input,
                InputMatcher.CurrentSpeedModifier,
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
                player.ReplaceCurrentSpeedModifier(input.CurrentSpeedModifier);

                _synchronizator.UpdateComponentsForEntity(player,
                    GameComponentsLookup.CurrentSpeedModifier);
            }
        }
    }
}
