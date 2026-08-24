using Assets.Code.ClientPart.Networking;
using Entitas;
using Yrr.Utils;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class InputListenClientSystem : IExecuteSystem
    {
        private readonly IGroup<InputEntity> _inputs;
        private readonly IGroup<GameEntity> _players;
        private readonly ClientMessenger _messenger;

        public InputListenClientSystem(InputContext input, GameContext game, ClientMessenger messenger)
        {
            _inputs = input.GetGroup(InputMatcher.AllOf(
                InputMatcher.Input,
                InputMatcher.ClickedPosition));

            _players = game.GetGroup(GameMatcher.ClientPlayer);
            _messenger = messenger;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _players)
            {
                foreach (var input in _inputs)
                {
                    var clickPos = input.ClickedPosition;
                    var targetRotation = AnglesUtil.GetAngleDirectionY(player.LocalPosition, input.ClickedPosition);

                  //  _messenger.SendTargetRotationToServer(targetRotation);
                }
            }
        }
    }
}
