using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;


namespace Assets.Code.UI.Screens.Wallet
{
    public sealed class WalletScreen : ScreenBase<WalletPresenter, WalletView>
    {
        public WalletScreen(IScreenViewsProvider viewsProvider,
            WalletPresenter presenter, LayerUI_Popups screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }
}
