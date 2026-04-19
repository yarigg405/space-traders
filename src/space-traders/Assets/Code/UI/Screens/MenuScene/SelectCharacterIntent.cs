using Assets.Code.ClientPart.Networking;
using Assets.Code.Networking;
using Assets.Code.UI.Infrastructure.Impl;
using Assets.Code.UI.Infrastructure.Interfaces;
using Cysharp.Threading.Tasks;
using System.Threading;


namespace Assets.Code.UI.Screens.MenuScene
{
    public sealed class SelectCharacterIntent : IAsyncNavigationIntent
    {
        private readonly ClientMessenger _clientMessenger;
        private readonly IUIManager _uiManager;
        private readonly AuthentificationContainer _authentification;

        public SelectCharacterIntent(ClientMessenger clientMessenger,
            IUIManager uiManager, AuthentificationContainer authentificationContainer)
        {
            _clientMessenger = clientMessenger;
            _uiManager = uiManager;
            _authentification = authentificationContainer;
        }

        async UniTask<object> IAsyncNavigationIntent.Load(CancellationToken token)
        {
            try
            {
                _uiManager.OpenModal<AwaitServerResponsePopup>();
                var charactersData = await _clientMessenger
                    .RequestForCharacters(_authentification.Login, _authentification.Password, token);
                token.ThrowIfCancellationRequested();
                return charactersData;
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
