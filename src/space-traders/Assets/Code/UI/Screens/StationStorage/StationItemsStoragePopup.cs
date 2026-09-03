using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;


namespace Assets.Code.UI.Screens.StationStorage
{
    public sealed class StationItemsStoragePopup : ScreenBase<StationItemsStoragePresenter, StationItemsStorageView>
    {
        public StationItemsStoragePopup(IScreenViewsProvider viewsProvider,
            StationItemsStoragePresenter presenter, LayerUI_Popups screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }
}
