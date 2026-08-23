using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.CurrentShipInfo;
using Assets.Code.UI.Screens.TradingMain;
using Assets.Code.UI.Screens.Wallet;


namespace Assets.Code.UI.Screens.GameMain
{
    public sealed class GameMainPresenter : IPresenter<GameMainView>
    {
        private readonly IUIManager _uiManager;

        public GameMainPresenter(IUIManager uiManager)
        {
            _uiManager = uiManager;
        }

        void IPresenter<GameMainView>.Show(GameMainView view, object args)
        {
            view.WalletBtn.onClick.AddListener(OpenWalletScreen);
            view.ShipInfoBtn.onClick.AddListener(OpenCurrentShipScreen);
            view.TradeBtn.onClick.AddListener(OpenTradeScreen);
        }

        void IPresenter<GameMainView>.Hide(GameMainView view)
        {
            view.WalletBtn.onClick.RemoveListener(OpenWalletScreen);
            view.ShipInfoBtn.onClick.RemoveListener(OpenCurrentShipScreen);
            view.TradeBtn.onClick.RemoveListener(OpenTradeScreen);
        }

        private void OpenWalletScreen()
        {
            _uiManager.OpenModal<WalletScreen>();
        }

        private void OpenCurrentShipScreen()
        {
            _uiManager.OpenModal<CurrentShipInfoScreen>();
        }

        private void OpenTradeScreen()
        {
            _uiManager.OpenModal<TradingMainPopup>();
        }
    }
}
