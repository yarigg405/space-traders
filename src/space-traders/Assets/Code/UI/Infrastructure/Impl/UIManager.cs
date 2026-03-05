using System;
using System.Collections.Generic;


namespace Assets.Code.UI.Infrastructure.Impl
{
    public sealed class UIManager : IUIManager
    {
        public event Action<IScreen> OnScreenOpened;
        public event Action<IScreen> OnScreenClosed;

        private readonly IScreensProvider _screensProvider;

        private readonly HashSet<Type> _openedScreens = new();

        public UIManager(IScreensProvider screensProvider)
        {
            _screensProvider = screensProvider;
        }

        public void OpenScreen<TScreen>(object args = null) where TScreen : IScreen
        {
            var screen = _screensProvider.GetScreen<TScreen>();
            screen.Show(args);
            OnScreenOpened?.Invoke(screen);
            _openedScreens.Add(screen.GetType());
        }

        public void CloseScreen<TScreen>() where TScreen : IScreen
        {
            var screen = _screensProvider.GetScreen<TScreen>();
            screen.Hide();
            OnScreenClosed?.Invoke(screen);
            _openedScreens.Remove(screen.GetType());
        }

        bool IUIManager.IsScreenOpened<TScreen>()
        {
            return _openedScreens.Contains(typeof(TScreen));
        }
    }
}
