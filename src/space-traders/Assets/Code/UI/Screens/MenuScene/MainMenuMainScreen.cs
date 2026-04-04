using Assets.Code.UI.Infrastructure;
using Assets.Code.UI.Screens.MainMenu;
using Assets.Code.UI.Screens.MenuScene;


namespace Assets.Code.UI.Screens
{
    public sealed class MainMenuMainScreen : ScreenBase<MainMenuMainPresenter, MainMenuMainView>
    {
        public MainMenuMainScreen(IScreenViewsProvider viewsProvider,
            MainMenuMainPresenter presenter, LayerUI_Screens screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }
}
