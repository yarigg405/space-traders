using UnityEngine;


namespace Yrr.UI.Elements
{
    internal abstract class CustomButtonStateObserver : MonoBehaviour
    {
        [SerializeField] protected CustomButton CustomButton;


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!CustomButton)
                CustomButton = GetComponent<CustomButton>();
        }
#endif


        private void OnEnable()
        {
            CustomButton.CurrentState.OnChange += OnButtonStateChanged;
            OnButtonStateChanged(CustomButton.CurrentState);
        }

        private void OnDisable()
        {
            CustomButton.CurrentState.OnChange -= OnButtonStateChanged;
        }

        protected abstract void OnButtonStateChanged(CustomButtonState state);
    }
}
