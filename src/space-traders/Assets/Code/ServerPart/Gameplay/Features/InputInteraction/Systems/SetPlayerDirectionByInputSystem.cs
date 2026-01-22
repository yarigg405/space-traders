using Entitas;
using Yrr.Utils;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class SetPlayerDirectionByInputSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly IGroup<InputEntity> _inputs;

        public SetPlayerDirectionByInputSystem(GameContext game, InputContext input)
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
                        player.isBraking = false;

                        break;
                    }
                }
            }
        }
    }
}
