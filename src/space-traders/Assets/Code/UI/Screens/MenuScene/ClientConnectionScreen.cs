using Assets.Code.UI.Infrastructure;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.MainMenu;
using Assets.Code.UI.Screens.MenuScene;


namespace Assets.Code.UI.Screens
{
    public sealed class ClientConnectionScreen : ScreenBase<ClientConnectionPresenter, ClientConnectionView>
    {
        public ClientConnectionScreen(IScreenViewsProvider viewsProvider,
            ClientConnectionPresenter presenter, LayerUI_Screens screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }
}
