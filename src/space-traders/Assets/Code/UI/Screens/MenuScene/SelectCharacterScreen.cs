using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;
using Assets.Code.UI.Screens.MainMenu;
using Assets.Code.UI.Screens.MenuScene;


namespace Assets.Code.UI.Screens
{
    public sealed class SelectCharacterScreen : ScreenBase<SelectCharacterPresenter, SelectCharacterView>
    {
        public SelectCharacterScreen(IScreenViewsProvider viewsProvider,
            SelectCharacterPresenter presenter, LayerUI_Screens screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }
}
