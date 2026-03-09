using TMPro;
using UnityEngine;


namespace Yrr.UI.Elements
{
    internal sealed class CustomButtonTmpColorChanger : CustomButtonStateObserver
    {
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _highlightedColor;
        [SerializeField] private Color _pressedColor;


        [Space]
        [SerializeField] private TextMeshProUGUI _text;

        protected override void OnButtonStateChanged(CustomButtonState state)
        {
            switch (state)
            {
                case CustomButtonState.Pressed:
                    {
                        _text.color = _pressedColor;
                    }
                    break;
                case CustomButtonState.Highlighted:
                    {
                        _text.color = _highlightedColor;
                    }
                    break;

                default:
                    {
                        _text.color = _normalColor;
                    }
                    break;
            }
        }
    }
}
