using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;


namespace Assets.Code.UI.Screens.StationsInventory
{
    public sealed class AllAssetsPopup : ScreenBase<AllAssetsPresenter, AllAssetsView>
    {
        public AllAssetsPopup(IScreenViewsProvider viewsProvider,
            AllAssetsPresenter presenter, LayerUI_Popups screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }
}
