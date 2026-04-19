using Assets.Code.Networking;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.MainMenu;
using Assets.Code.UI.Screens.MessageBox;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Assets.Code.UI.Screens.MenuScene
{
    public sealed class ClientConnectionPresenter : IPresenter<ClientConnectionView>
    {
        private readonly IUIManager _uIManager;
        private readonly NetworkManager _networkManager;
        private readonly ILifetimeCancellationToken _lcts;
        private readonly AuthentificationContainer _authentification;

        private const string _playerPrefsKey = "PlayerLogin";

        private CancellationTokenSource _cts;
        private ClientConnectionView _view;

        public ClientConnectionPresenter(IUIManager uIManager, 
            NetworkManager networkManager, ILifetimeCancellationToken cts, 
            AuthentificationContainer authentification)
        {
            _uIManager = uIManager;
            _networkManager = networkManager;
            _lcts = cts;
            _authentification = authentification;
        }

        void IPresenter<ClientConnectionView>.Show(ClientConnectionView view, object args)
        {
            _cts = new();
            _view = view;

            _view.PlayerNameIF.text = GetSavedPlayerLogin();

            view.CloseButton.onClick.AddListener(ClickOnBack);
            view.ConnectButton.onClick.AddListener(ClickOnConnect);

            view.Show();
        }

        void IPresenter<ClientConnectionView>.Hide(ClientConnectionView view)
        {
            SavePlayerLogin();
            _cts?.Cancel();
            _cts?.Dispose();
            view.Hide();

            view.CloseButton.onClick.RemoveListener(ClickOnBack);
            view.ConnectButton.onClick.RemoveListener(ClickOnConnect);
        }


        private void ClickOnConnect()
        {
            var ipAddress = _view.HostAddressIF.text;
            var port = ushort.TryParse(_view.HostPortIF.text, out var r) ? r : (ushort)0;
            var password = _view.HostPasswordIF.text;

            TryConnectAsync(ipAddress, port, password).Forget();
        }

        private async UniTask TryConnectAsync(string ipAddress, ushort port, string serverPassword)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, _lcts.Token);
            var token = linkedCts.Token;
            try
            {
                _uIManager.CloseModal<MessageBoxPopup>();
                _uIManager.OpenModal<AwaitServerResponsePopup>();
                _authentification.Login = _view.PlayerNameIF.text;
                _authentification.Password = serverPassword;

                await _networkManager.StartClient(ipAddress, port, token);
                _uIManager.GoToScreen<SelectCharacterScreen>();
            }

            catch (OperationCanceledException) 
            {
                _networkManager.StopClient();
            }

            catch (Exception ex)
            {
                _networkManager.StopClient();

                var data = MessageBoxPopup.CreateData(ex.Message);
                _uIManager.OpenModal<MessageBoxPopup>(data);
            }

            finally
            {
                _uIManager.CloseModal<AwaitServerResponsePopup>();
            }
        }

        private void ClickOnBack()
        {
            _uIManager.BackToPreviousScreen();
        }

        private string GetSavedPlayerLogin()
        {
            if (!PlayerPrefs.HasKey(_playerPrefsKey))
            {
                PlayerPrefs.SetString(_playerPrefsKey, "Player" + Random.Range(64, 512));
            }

            return PlayerPrefs.GetString(_playerPrefsKey);
        }

        private void SavePlayerLogin()
        {
            if (!_view) return;

            PlayerPrefs.SetString(_playerPrefsKey, _view.PlayerNameIF.text);
            PlayerPrefs.Save();
        }
    }
}
