using UnityEngine;


namespace Yrr.UI.Elements
{
    internal sealed class CustomButtonSpriteChanger : CustomButtonStateObserver
    {
        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _pressedSprite;


        protected override void OnButtonStateChanged(CustomButtonState state)
        {
            if (state == CustomButtonState.Pressed)
            {
                CustomButton.image.sprite = _pressedSprite;
            }

            else
            {
                CustomButton.image.sprite = _normalSprite;
            }
        }
    }
}
