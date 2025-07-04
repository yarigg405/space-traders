using Entitas;


namespace Assets.Code.Gameplay.InputInteraction.Systems
{
    internal sealed class EmitInputSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _inputs;
        private readonly InputDataContainer _inputContainer;

        internal EmitInputSystem(GameContext game, InputDataContainer inputContainer)
        {
            _inputContainer = inputContainer;
            _inputs = game.GetGroup(GameMatcher.Input);
        }

        void IExecuteSystem.Execute()
        {
            foreach (var input in _inputs)
            {
                if (_inputContainer.IsChanged)
                {
                    input.ReplaceClickedPosition(_inputContainer.GetInputValue());
                }
                else if (input.hasClickedPosition)
                {
                    input.RemoveClickedPosition();
                }
            }
        }
    }
}
