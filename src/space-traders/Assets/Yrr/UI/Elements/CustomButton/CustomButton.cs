using UnityEngine;
using UnityEngine.UI;
using Yrr.Utils;


namespace Yrr.UI.Elements
{
    public sealed class CustomButton : Button
    {
        public ReactiveValue<CustomButtonState> CurrentState { get; private set; } = new();

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            Debug.Log("State: " + state);

            base.DoStateTransition(state, instant);

            var number = (int)state;
            var buttonState = (CustomButtonState)number;
            CurrentState.SetValue(buttonState);

            Debug.Log("ButtonState: " + CurrentState.Value);
        }
    }

    public enum CustomButtonState
    {
        Normal,
        Highlighted,
        Pressed,
        Selected,
        Disabled,
    }
}
