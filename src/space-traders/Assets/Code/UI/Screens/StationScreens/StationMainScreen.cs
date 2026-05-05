using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;
using Assets.Code.UI.Screens.StationScreens;


namespace Assets.Code.UI.Screens
{
    public sealed class StationMainScreen : ScreenBase<StationMainPresenter, StationMainView>
    {
        public StationMainScreen(IScreenViewsProvider viewsProvider,
            StationMainPresenter presenter, LayerUI_Screens screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }
}
