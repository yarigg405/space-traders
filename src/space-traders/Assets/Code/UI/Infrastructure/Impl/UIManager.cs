using System;


namespace Assets.Code.UI.Infrastructure.Impl
{
    public sealed class UIManager : IUIManager
    {
        public event Action<IScreen> OnScreenOpened;
        public event Action<IScreen> OnScreenClosed;

        private readonly IScreensProvider _screensProvider;

        public UIManager(IScreensProvider screensProvider)
        {
            _screensProvider = screensProvider;
        }

        public void OpenScreen<TScreen>(object args = null) where TScreen : IScreen
        {
            var screen = _screensProvider.GetScreen<TScreen>();
            screen.Show(args);
            OnScreenOpened?.Invoke(screen);
        }

        public void CloseScreen<TScreen>() where TScreen : IScreen
        {
            var screen = _screensProvider.GetScreen<TScreen>();
            screen.Hide();
            OnScreenClosed?.Invoke(screen);
        }
    }
}
