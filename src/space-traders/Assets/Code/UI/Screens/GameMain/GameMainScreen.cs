using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;


namespace Assets.Code.UI.Screens.GameMain
{
    public sealed class GameMainScreen : ScreenBase<GameMainPresenter, GameMainView>
    {
        public GameMainScreen(IScreenViewsProvider viewsProvider,
            GameMainPresenter presenter, LayerUI_HUD screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }
}
