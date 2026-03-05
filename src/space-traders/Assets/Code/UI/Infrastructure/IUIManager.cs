using System;

namespace Assets.Code.UI.Infrastructure
{
    public interface IUIManager
    {
        event Action<IScreen> OnScreenClosed;
        event Action<IScreen> OnScreenOpened;

        void CloseScreen<TScreen>() where TScreen : IScreen;
        void OpenScreen<TScreen>(object args = null) where TScreen : IScreen;
    }
}