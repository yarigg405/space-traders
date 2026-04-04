using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.StatesInfrastructure;
using Assets.Code.UI;
using Assets.Code.UI.Screens;


namespace Assets.Code.Infrastructure.States.GameStates
{
    internal sealed class MenuSceneState : GameState
    {
        private readonly IScenesLoader _scenesLoader;
        private readonly IUIManager _uiManager;

        public MenuSceneState(IScenesLoader scenesLoader, IUIManager uiManager)
        {
            _scenesLoader = scenesLoader;
            _uiManager = uiManager;
        }

        public override void Enter()
        {
            _scenesLoader.LoadScene(SceneNames.MenuScene, LoadMenu);
        }

        private void LoadMenu()
        {
            _uiManager.GoToScreen<MainMenuMainScreen>();
        }
    }
}
