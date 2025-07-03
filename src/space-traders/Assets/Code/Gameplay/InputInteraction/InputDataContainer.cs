using UnityEngine;


namespace Assets.Code.Gameplay.InputInteraction
{
    public sealed class InputDataContainer
    {
        private Vector3 _inputValue;

        public bool IsChanged { get; private set; }

        public void SetInput(Vector3 inputValue)
        {
            _inputValue = inputValue;
            IsChanged = true;
        }

        public Vector3 GetInputValue()
        {
            IsChanged = false;
            return _inputValue;
        }
    }
}
