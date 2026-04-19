using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Layers;
using Assets.Code.UI.Screens.MainMenu;
using Assets.Code.UI.Screens.MenuScene;


namespace Assets.Code.UI.Screens
{
    public sealed class CreateCharacterScreen : ScreenBase<CreateCharacterPresenter, CreateCharacterView>
    {
        public CreateCharacterScreen(IScreenViewsProvider viewsProvider,
            CreateCharacterPresenter presenter, LayerUI_Screens screenRoot)
            : base(viewsProvider, presenter, screenRoot) { }
    }
}
