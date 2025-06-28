using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Yrr.UI.Infrastructure;


namespace Yrr.UI
{
    public abstract class UIScreen : MonoBehaviour, IUiSimpleScreen
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private ScreenEvents screenEvents;
        private event Action _closingCallback;

        private bool _isClosing;
        protected IUIManager UIManager;

        public virtual bool IsModal => false;


        void IUIScreen.SetupUiManager(IUIManager manager)
        {
            UIManager = manager;
        }

        public void Show(Action callback)
        {
            _isClosing = false;
            ShowProcedure();
            _closingCallback = callback;
            OnShow();
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

        protected virtual void OnShow() { }


        protected virtual void HidingProcedure()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnHide() { }
    }

    [Serializable]
    public struct ScreenEvents
    {
        public UnityEvent OnShow;
        public UnityEvent OnHide;
    }
}