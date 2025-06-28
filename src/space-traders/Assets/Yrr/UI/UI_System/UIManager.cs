using System;
using System.Collections.Generic;
using Yrr.UI;
using Yrr.UI.Infrastructure;
using UnityEngine;


namespace Assets.Yrr.UI.UI_System
{
    public sealed class UIManager : IUIManager
    {
        private IScreenSupplier<Type> _supplier;

        public UIManager(ScreensSupplier supplier)
        {
            _supplier = supplier;
        }

        public event Action<IUIScreen> OnScreenOpen;
        public event Action<IUIScreen> OnScreenHide;

        private IUiSimpleScreen _currentScreen;
        private Stack<IUiSimpleScreen> _screensHistory = new();
        private Queue<IUIScreen> _queuedModals = new();


        public void Show<TScreen>(Action closingCallback = null) where TScreen : UIScreen
        {
            var screen = _supplier.GetScreen<TScreen>() as IUiSimpleScreen;
            if (screen.IsModal)
            {
                OpenModal(screen, closingCallback);
            }

            else
            {
                ChangeWindow(screen, closingCallback);
                if (screen is MainScreen)
                    ClearHistory();
            }
        }

        public void Show<TScreen, TPayload>(TPayload payload, Action closingCallback = null) where TScreen : UIScreenPayload<TPayload>
        {
            var screen = _supplier.GetScreen<TScreen>() as IPayloadScreen<TPayload>;
            OpenModal(screen, payload, closingCallback);

            //cant do payload screen not modal, because screens history (cant  history payload)
        }

        public void Hide<TScreen>() where TScreen : IUIScreen
        {
            var screen = _supplier.GetScreen<TScreen>();
            HideInternal(screen);
        }

        void IUIManager.Hide(IUIScreen screen)
        {
            HideInternal(screen);
        }

        private void ChangeWindow(IUiSimpleScreen screen, Action callback)
        {
            CloseCurrent();
            _currentScreen = screen;
            screen.Show(callback);
        }

        private void OpenModal(IUiSimpleScreen screen, Action callback)
        {
            screen.Show(callback);
        }

        private void OpenModal<TPayload>(IPayloadScreen<TPayload> screen, TPayload payload, Action callback)
        {
            screen.Show(payload, callback);
        }

        private void CloseCurrent()
        {
            if (_currentScreen != null)
            {
                _screensHistory.Push(_currentScreen);
                _currentScreen.Hide();
                OnScreenHide?.Invoke(_currentScreen);
            }
        }

        private void HideInternal(IUIScreen uIScreen)
        {
            if (uIScreen.IsModal)
            {
                uIScreen.Hide();
            }

            else
            {
                if (_currentScreen is IUiSimpleScreen simplescreen)
                {
                    _currentScreen.Hide();
                    OnScreenHide?.Invoke(_currentScreen);

                    _currentScreen = _screensHistory.Pop();
                    _currentScreen.Show(null);
                }

                else
                {
                    uIScreen.Hide();
                    //cant do payload screen not modal, because screens history (cant  history payload)
                }
            }
        }

        private void ClearHistory()
        {
            _screensHistory.Clear();
        }
    }
}
