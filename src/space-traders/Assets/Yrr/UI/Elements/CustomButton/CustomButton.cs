using UnityEngine.UI;
using Yrr.Utils;


namespace Yrr.UI.Elements
{
    public sealed class CustomButton : Button
    {
        public ReactiveValue<CustomButtonState> CurrentState { get; private set; }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            CurrentState.SetValue((CustomButtonState)state);
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
