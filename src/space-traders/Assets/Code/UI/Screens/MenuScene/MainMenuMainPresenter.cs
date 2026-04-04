using Assets.Code.UI.Infrastructure;
using Assets.Code.UI.Screens.MainMenu;
using UnityEngine;


namespace Assets.Code.UI.Screens.MenuScene
{
    public sealed class MainMenuMainPresenter : IPresenter<MainMenuMainView>
    {
        private readonly IUIManager _uIManager;

        public MainMenuMainPresenter(IUIManager uIManager)
        {
            _uIManager = uIManager;
        }
        void IPresenter<MainMenuMainView>.Show(MainMenuMainView view)
        {
            view.CloseButton.onClick.AddListener(ClickOnQuit);
            view.StartGameBtn.onClick.AddListener(ClickOnStartGame);
            view.JoinGameBtn.onClick.AddListener(ClickOnJoinGame);

            view.Show();
        }

        void IPresenter<MainMenuMainView>.Hide(MainMenuMainView view)
        {
            view.Hide();

            view.CloseButton.onClick.RemoveListener(ClickOnQuit);
            view.StartGameBtn.onClick.RemoveListener(ClickOnStartGame);
            view.JoinGameBtn.onClick.RemoveListener(ClickOnJoinGame);
        }

        private void ClickOnStartGame()
        {

        }

        private void ClickOnJoinGame()
        {
            _uIManager.GoToScreen<ClientConnectionScreen>();
        }

        private void ClickOnQuit()
        {
            Application.Quit();
        }
    }
}
