using Assets.Code.Networking;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.MainMenu;
using Assets.Code.UI.Screens.MessageBox;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;


namespace Assets.Code.UI.Screens.MenuScene
{
    public sealed class MainMenuMainPresenter : IPresenter<MainMenuMainView>
    {
        private readonly IUIManager _uIManager;
        private readonly NetworkManager _networkManager;
        private readonly ILifetimeCancellationToken _cts;
        private readonly AuthentificationContainer _authentification;

        private MainMenuMainView _view;

        public MainMenuMainPresenter(IUIManager uIManager, NetworkManager networkManager,
            ILifetimeCancellationToken cts, AuthentificationContainer authentification)
        {
            _uIManager = uIManager;
            _networkManager = networkManager;
            _cts = cts;
            _authentification = authentification;
        }
        void IPresenter<MainMenuMainView>.Show(MainMenuMainView view, object args)
        {
            _view = view;

            view.CloseButton.onClick.AddListener(ClickOnQuit);
            view.StartGameBtn.onClick.AddListener(ClickOnStartGame);
            view.JoinGameBtn.onClick.AddListener(ClickOnJoinGame);

            _networkManager.Cleanup();
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
            try
            {
                _uIManager.CloseModal<MessageBoxPopup>();
                _uIManager.OpenModal<AwaitServerResponsePopup>();
                _authentification.Login = "_server";
                _authentification.Password = serverPassword;

                await _networkManager.StartHost(port, serverPassword, _cts.Token);
                _uIManager.GoToScreen<SelectCharacterScreen>();
            }

            catch (OperationCanceledException)
            {
            }

            catch (Exception ex)
            {
                var data = MessageBoxPopup.CreateData(ex.Message);
                _uIManager.OpenModal<MessageBoxPopup>(data);
            }

            finally
            {
                _uIManager.CloseModal<AwaitServerResponsePopup>();
            }
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
