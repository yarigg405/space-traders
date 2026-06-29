using Assets.Code.UI.Infrastructure.Interfaces;


namespace Assets.Code.UI.Screens.TradingMain
{
    public sealed class TradingMainPresenter : IPresenter<TradingMainView>
    {
        private readonly IUIManager _uiManager;

        public TradingMainPresenter(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        void IPresenter<TradingMainView>.Show(TradingMainView view, object args)
        {
            view.CloseButton.onClick.AddListener(ClickOnClose);
        }

        void IPresenter<TradingMainView>.Hide(TradingMainView view)
        {
            view.CloseButton.onClick.RemoveListener(ClickOnClose);
        }

        private void ClickOnClose()
        {
            _uiManager.CloseModal<TradingMainPopup>();
        }
    }
}
