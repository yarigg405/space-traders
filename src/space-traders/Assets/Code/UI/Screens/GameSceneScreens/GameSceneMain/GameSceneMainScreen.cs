using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;


namespace Assets.Code.UI.Screens.GameSceneScreens.GameSceneMain
{
    public sealed class GameSceneMainScreen : ScreenBase<GameSceneMainPresenter, GameSceneMainView>
    {
        public GameSceneMainScreen(IScreenViewsProvider viewsProvider,
            GameSceneMainPresenter presenter, LayerUI_Screens screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }
}
