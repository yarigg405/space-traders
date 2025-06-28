using System;
using UnityEngine;
using UnityEngine.UI;
using Yrr.UI.Infrastructure;


namespace Yrr.UI
{
    public abstract class UIScreenPayload<TPayload> : MonoBehaviour, IPayloadScreen<TPayload>
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private ScreenEvents screenEvents;
        private event Action _closingCallback;

        private bool _isClosing;
        protected IUIManager UIManager;

        [field: SerializeField] public bool IsModal { get; private set; }
        public Type GetPayloadType => typeof(TPayload);

        void IUIScreen.SetupUiManager(IUIManager manager)
        {
            UIManager = manager;
        }

        public void Show(TPayload payload, Action callback)
        {
            _isClosing = false;
            ShowProcedure();
            _closingCallback = callback;
            OnShow(payload);
            screenEvents.OnShow?.Invoke();

            if (_closeButton)
                _closeButton.onClick.AddListener(ClickOnClose);
        }

        private void ClickOnClose()
        {
            UIManager.Hide(this);
        }

        void IUIScreen.Hide()
        {
            if (!gameObject.activeSelf) return;
            if (_isClosing) return;
            _isClosing = true;

            if (_closeButton)
                _closeButton.onClick.RemoveListener(ClickOnClose);

            OnHide();
            screenEvents.OnHide?.Invoke();
            _closingCallback?.Invoke();
            _closingCallback = null;
            HidingProcedure();
        }

        protected virtual void ShowProcedure()
        {
            gameObject.SetActive(true);
        }

        protected virtual void OnShow(TPayload payload) { }


        protected virtual void HidingProcedure()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnHide() { }
    }
}
