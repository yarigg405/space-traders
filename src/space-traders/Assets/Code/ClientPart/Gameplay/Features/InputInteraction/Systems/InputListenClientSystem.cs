using Assets.Code.ClientPart.Networking;
using Entitas;
using Yrr.Utils;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class InputListenClientSystem : IExecuteSystem
    {
        private readonly IGroup<InputEntity> _inputs;
        private readonly IGroup<GameEntity> _players;

        public InputListenClientSystem(InputContext input, GameContext game)
        {
            _inputs = input.GetGroup(InputMatcher.AllOf(
                InputMatcher.Input,
                InputMatcher.ClickedPosition));

            _players = game.GetGroup(GameMatcher.ClientPlayer);
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _players)
            {
                foreach (var input in _inputs)
                {
                    var clickPos = input.ClickedPosition;
                    var targetRotation = AnglesUtil.GetAngleDirectionY(player.LocalPosition, input.ClickedPosition);

                    ClientMessenger.SendTargetRotationToServer(targetRotation);
                }
            }
        }
    }
}
