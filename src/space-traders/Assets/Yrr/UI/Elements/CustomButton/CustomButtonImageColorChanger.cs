using UnityEngine;
using UnityEngine.UI;


namespace Yrr.UI.Elements
{
    internal sealed class CustomButtonImageColorChanger : CustomButtonStateObserver
    {
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _highlightedColor;
        [SerializeField] private Color _pressedColor;


        [Space]
        [SerializeField] private Image _image;

        protected override void OnButtonStateChanged(CustomButtonState state)
        {
            switch (state)
            {
                case CustomButtonState.Pressed:
                    {
                        _image.color = _pressedColor;
                    }
                    break;
                case CustomButtonState.Highlighted:
                    {
                        _image.color = _highlightedColor;
                    }
                    break;

                default:
                    {
                        _image.color = _normalColor;
                    }
                    break;
            }
        }
    }
}
