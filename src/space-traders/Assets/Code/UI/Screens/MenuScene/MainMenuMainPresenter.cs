using Assets.Code.UI.Infrastructure;
using Assets.Code.UI.Screens.MainMenu;
using UnityEngine;


namespace Assets.Code.UI.Screens.MenuScene
{
    public sealed class MainMenuMainPresenter
    {
        private readonly IUIManager _uIManager;

        public MainMenuMainPresenter(IUIManager uIManager)
        {
            _uIManager = uIManager;
        }

        internal void Show(MainMenuMainView view)
        {
            view.CloseButton.onClick.AddListener(ClickOnQuit);
            view.StartGameBtn.onClick.AddListener(ClickOnStartGame);
            view.JoinGameBtn.onClick.AddListener(ClickOnJoinGame);

            view.Show();
        }

        internal void Hide(MainMenuMainView view)
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
          
        }

        private void ClickOnQuit()
        {
            Application.Quit();
        }
    }
}
