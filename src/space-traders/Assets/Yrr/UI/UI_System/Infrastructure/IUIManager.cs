using System;


namespace Yrr.UI.Infrastructure
{
    public interface IUIManager
    {
        void Show<TScreen, TPayload>(TPayload payload, Action closingCallback = null) where TScreen : UIScreenPayload<TPayload>;
        void Show<TScreen>(Action closingCallback = null) where TScreen : UIScreen;
        void Hide<TScreen>() where TScreen : IUIScreen;
        void Hide(IUIScreen uIScreen);
    }
}
