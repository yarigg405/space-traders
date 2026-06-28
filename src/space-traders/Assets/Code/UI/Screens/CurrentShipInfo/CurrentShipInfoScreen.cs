using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;


namespace Assets.Code.UI.Screens.CurrentShipInfo
{
    public sealed class CurrentShipInfoScreen : ScreenBase<CurrentShipInfoPresenter, CurrentShipInfoView>
    {
        public CurrentShipInfoScreen(IScreenViewsProvider viewsProvider,
            CurrentShipInfoPresenter presenter, LayerUI_Popups screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }
}
