using Assets.Code.Common.Extensions;
using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class SetPlayerDirectionByInputSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly IGroup<InputEntity> _inputs;

        private readonly EntitiesSynchronizator _synchronizator;

        public SetPlayerDirectionByInputSystem(GameContext game,
            InputContext input, EntitiesSynchronizator synchronizator)
        {
            _players = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Player,
                GameMatcher.PlayerNetworkId
                ));

            _inputs = input.GetGroup(InputMatcher.AllOf(
                InputMatcher.Input,
                InputMatcher.TargetRotation,
                InputMatcher.InputPlayerTarget
                ));
            _synchronizator = synchronizator;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var input in _inputs)
            {
                foreach (var player in _players)
                {
                    if (input.InputPlayerTarget == player.PlayerNetworkId)
                    {
                        player.ReplaceTargetRotation(input.TargetRotation);
                        player.ReplaceBraking(false);

                        _synchronizator.UpdateComponentsForEntity(player,
                            GameComponentsLookup.TargetRotation,
                            GameComponentsLookup.Braking);

                        break;
                    }
                }
            }
        }
    }
}
