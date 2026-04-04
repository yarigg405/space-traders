using System;

namespace Assets.Code.UI.Infrastructure.Interfaces
{
    public interface IUIManager
    {
        event Action<IScreen> OnScreenClosed;
        event Action<IScreen> OnScreenOpened;

        void GoToScreen<TScreen>(object args = null) where TScreen : IScreen;
        void BackToPreviousScreen();

        void OpenModal<TPopup>(object args = null) where TPopup : IScreen;
        void CloseModal<TPopup>() where TPopup : IScreen;
        void CloseAllModals();

        void ClearHistory();
    }
}