using Assets.Code.Networking;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.MainMenu;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Assets.Code.UI.Screens.MenuScene
{
    public sealed class MainMenuMainPresenter : IPresenter<MainMenuMainView>
    {
        private readonly IUIManager _uIManager;
        private readonly NetworkManager _networkManager;
        private readonly ICancellationToken _cts;
        private MainMenuMainView _view;

        public MainMenuMainPresenter(IUIManager uIManager, NetworkManager networkManager,
            ICancellationToken cts)
        {
            _uIManager = uIManager;
            _networkManager = networkManager;
            _cts = cts;
        }
        void IPresenter<MainMenuMainView>.Show(MainMenuMainView view, object args)
        {
            _view = view;

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
            var port = ushort.TryParse(_view.ServerPortIF.text, out var r) ? r : (ushort)0;
            var password = _view.ServerPasswordIF.text;

            StartHostAsync(port, password).Forget();
        }

        private async UniTask StartHostAsync(ushort port, string serverPassword)
        {
            _uIManager.OpenModal<AwaitServerResponsePopup>();
            await _networkManager.StartHost(port, serverPassword, _cts.Token);
            await UniTask.NextFrame();
            _uIManager.CloseModal<AwaitServerResponsePopup>();
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
