using Assets.Code.UI.Infrastructure;
using Assets.Code.UI.Screens.MainMenu;
using Assets.Code.UI.Screens.MenuScene;


namespace Assets.Code.UI.Screens
{
    public sealed class MainMenuMainScreen : IScreen
    {
        private readonly IScreenViewsProvider _viewsProvider;
        private readonly MainMenuMainPresenter _presenter;
        private readonly LayerUI_Screens _screenRoot;

        public MainMenuMainScreen(IScreenViewsProvider viewsProvider,
            MainMenuMainPresenter presenter, LayerUI_Screens screenRoot)
        {
            _viewsProvider = viewsProvider;
            _presenter = presenter;
            _screenRoot = screenRoot;
        }

        void IScreen.Show(object args)
        {
            var view = _viewsProvider.GetView<MainMenuMainView>();

            _presenter.Show(view);
            _screenRoot.ShowView(view);
        }

        void IScreen.Hide()
        {
            var view = _viewsProvider.GetView<MainMenuMainView>();
            _presenter.Hide(view);
            _screenRoot.HideView(view);
        }
    }
}
