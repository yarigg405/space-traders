using Assets.Code.Common.StaticData;
using Assets.Code.Common.TradingSystem;
using Assets.Code.UI.Infrastructure.Interfaces;


namespace Assets.Code.UI.Screens.TradingMain
{
    public sealed class TradingMainPresenter : IPresenter<TradingMainView>
    {
        private readonly IUIManager _uiManager;
        private readonly TradeItemsCategoryConfig _categoriesConfig;
        private readonly IItemsCatalog _itemsCatalog;

        public TradingMainPresenter(IUIManager uiManager, TradeItemsCategoryConfig categoriesConfig, IItemsCatalog itemsCatalog)
        {
            _uiManager = uiManager;
            _categoriesConfig = categoriesConfig;
            _itemsCatalog = itemsCatalog;
        }

        void IPresenter<TradingMainView>.Show(TradingMainView view, object args)
        {
            view.SetupCategories(_categoriesConfig.GetAllCategories(), _itemsCatalog.GetAllItems());
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
