using Assets.Code.UI.Infrastructure.Impl;
using Assets.Code.UI.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;


namespace Assets.Code.UI
{
    public sealed class UIManager : IUIManager
    {
        public event Action<IScreen> OnScreenOpened;
        public event Action<IScreen> OnScreenClosed;

        private readonly IScreensProvider _screensProvider;
        private readonly UiNavigationStack _navigation;

        private readonly HashSet<Type> _openedScreens = new();
        private readonly HashSet<Type> _openedModals = new();

        public UIManager(IScreensProvider screensProvider)
        {
            _screensProvider = screensProvider;
            _navigation = new(screensProvider);
        }


        public void GoToScreen<TScreen>(object args = null) where TScreen : IScreen
        {
            var (opened, closed) = _navigation.Push(typeof(TScreen), args);

            if (closed != null)
            {
                _openedScreens.Remove(closed.GetType());
                OnScreenClosed?.Invoke(closed);
            }

            _openedScreens.Add(opened.GetType());
            OnScreenOpened?.Invoke(opened);
        }

        public void BackToPreviousScreen()
        {
            var (closed, opened) = _navigation.Pop();

            if (closed == null)
                return;

            _openedScreens.Remove(closed.GetType());
            OnScreenClosed?.Invoke(closed);

            if (opened != null)
            {
                _openedScreens.Add(opened.GetType());
                OnScreenOpened?.Invoke(opened);
            }
        }


        public void OpenModal<TPopup>(object args = null) where TPopup : IScreen
        {
            var type = typeof(TPopup);

            if (_openedModals.Contains(type))
                return;

            var modal = _screensProvider.GetScreen<TPopup>();
            modal.Show(args);
            _openedModals.Add(type);
            OnScreenOpened?.Invoke(modal);
        }

        public void CloseModal<TPopup>() where TPopup : IScreen
        {
            var type = typeof(TPopup);

            if (!_openedModals.Contains(type))
                return;

            var modal = _screensProvider.GetScreen<TPopup>();
            modal.Hide();
            _openedModals.Remove(type);
            OnScreenClosed?.Invoke(modal);
        }

        public void CloseAllModals()
        {
            foreach (var type in _openedModals)
            {
                var modal = _screensProvider.GetScreen(type);
                modal.Hide();
                OnScreenClosed?.Invoke(modal);
            }

            _openedModals.Clear();
        }


        public void ClearHistory()
        {
            _navigation.Clear();
        }
    }
}
