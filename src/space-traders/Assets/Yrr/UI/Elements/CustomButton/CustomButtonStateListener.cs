using UnityEngine;


namespace Yrr.UI.Elements
{
    internal abstract class CustomButtonStateListener : MonoBehaviour
    {
        [SerializeField] private CustomButton _customButton;

        private void OnEnable()
        {
            _customButton.CurrentState.OnChange += OnStateChangedHandler;
            OnStateChangedHandler(_customButton.CurrentState);
        }

        private void OnDisable()
        {
            _customButton.CurrentState.OnChange -= OnStateChangedHandler;
        }

        private void OnStateChangedHandler(CustomButtonState state)
        {
            switch (state)
            {

                case CustomButtonState.Highlighted:
                    SetHighlighted(); break;

                case CustomButtonState.Pressed:
                    SetPressed(); break;

                case CustomButtonState.Selected:
                    SetSelected(); break;

                case CustomButtonState.Disabled:
                    SetDisabled(); break;

                default:
                    SetNormal(); break;

            }
        }

        protected virtual void SetNormal() { }
        protected virtual void SetHighlighted() { }
        protected virtual void SetPressed() { }
        protected virtual void SetSelected() { }
        protected virtual void SetDisabled() { }
    }
}
