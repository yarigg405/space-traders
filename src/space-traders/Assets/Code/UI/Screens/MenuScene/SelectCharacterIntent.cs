using Assets.Code.ClientPart.Networking;
using Assets.Code.Networking;
using Assets.Code.UI.Infrastructure.Impl;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.MessageBox;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;


namespace Assets.Code.UI.Screens.MenuScene
{
    public sealed class SelectCharacterIntent : IAsyncNavigationIntent
    {
        private readonly ClientMessenger _clientMessenger;
        private readonly IUIManager _uiManager;
        private readonly AuthenticationContainer _authentication;

        public SelectCharacterIntent(ClientMessenger clientMessenger,
            IUIManager uiManager, AuthenticationContainer authenticationContainer)
        {
            _clientMessenger = clientMessenger;
            _uiManager = uiManager;
            _authentication = authenticationContainer;
        }

        async UniTask<object> IAsyncNavigationIntent.Load(CancellationToken token)
        {
            try
            {
                _uiManager.OpenModal<AwaitServerResponsePopup>();
                var charactersData = await _clientMessenger
                    .RequestForCharacters(_authentication.Login, _authentication.Password, token);
                token.ThrowIfCancellationRequested();
                return charactersData;
            }
            catch (RequestFailedException ex)
            {
                var data = MessageBoxPopup.CreateData(ex.Message);
                _uiManager.OpenModal<MessageBoxPopup>(data);
                throw new OperationCanceledException();
            }
            finally
            {
                _uiManager.CloseModal<AwaitServerResponsePopup>();
            }
        }

        INavigationIntent IAsyncNavigationIntent.Create(object data)
        {
            return new OpenScreenIntent(typeof(SelectCharacterScreen), data);
        }
    }
}
