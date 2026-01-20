using Assets.Code.ClientPart.Networking;
using Entitas;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction.Systems
{
    internal sealed class InputListenClientSystem : IExecuteSystem
    {
        private readonly IGroup<InputEntity> _entities;

        public InputListenClientSystem(InputContext input)
        {
            _entities = input.GetGroup(InputMatcher.AllOf(
                InputMatcher.Input,
                InputMatcher.ClickedPosition));
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var clickPos = entity.ClickedPosition;
                ClientMessenger.SendClickInputToServer(clickPos);
            }
        }
    }
}
