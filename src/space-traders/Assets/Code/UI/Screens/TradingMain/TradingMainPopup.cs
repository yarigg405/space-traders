using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;


namespace Assets.Code.UI.Screens.TradingMain
{
    public sealed class TradingMainPopup : ScreenBase<TradingMainPresenter, TradingMainView>
    {
        public TradingMainPopup(IScreenViewsProvider viewsProvider,
            TradingMainPresenter presenter, LayerUI_Popups screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }
}
