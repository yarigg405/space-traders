using Assets.Code.ClientPart.Networking;
using Assets.Code.Networking;
using Assets.Code.Networking.Data;
using Assets.Code.UI.Infrastructure.Interfaces;
using Assets.Code.UI.Screens.MainMenu;
using Assets.Code.UI.Screens.MessageBox;
using Cysharp.Threading.Tasks;
using System.Threading;


namespace Assets.Code.UI.Screens.MenuScene
{
    public sealed class CreateCharacterPresenter : IPresenter<CreateCharacterView>
    {
        private readonly IUIManager _uiManager;
        private readonly AuthentificationContainer _authentification;
        private readonly ClientMessenger _clientMessenger;
        private readonly ILifetimeCancellationToken _lcts;

        private CancellationTokenSource _cts;
        private CreateCharacterView _view;

        public CreateCharacterPresenter(IUIManager uIManager,
            AuthentificationContainer authentification, ClientMessenger clientMessenger,
            ILifetimeCancellationToken cts)
        {
            _uiManager = uIManager;
            _authentification = authentification;
            _clientMessenger = clientMessenger;
            _lcts = cts;
        }

        void IPresenter<CreateCharacterView>.Show(CreateCharacterView view, object args)
        {
            _cts = new();
            _view = view;
            view.CreateCharacterButton.onClick.AddListener(ClickOnCreate);
            view.CloseButton.onClick.AddListener(ClickOnBack);
        }

        void IPresenter<CreateCharacterView>.Hide(CreateCharacterView view)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            view.CloseButton.onClick.RemoveListener(ClickOnBack);
        }

        private void ClickOnCreate()
        {
            CreateCharacterAsync().Forget();
        }

        private async UniTask CreateCharacterAsync()
        {
            using var linkedCts = CancellationTokenSource
                .CreateLinkedTokenSource(_cts.Token, _lcts.Token);
            var token = linkedCts.Token;

            try
            {
                _uiManager.CloseModal<MessageBoxPopup>();
                _uiManager.OpenModal<AwaitServerResponsePopup>();

                var character = new CharacterData
                {
                    Name = _view.CharacterNameIF.text
                };

                var result = await _clientMessenger
                    .RequestForCreateNewCharacter(_authentification.Login, character, token);

                token.ThrowIfCancellationRequested();
                _uiManager.BackToPreviousScreen();
            }

            catch (RequestFailedException e)
            {
                var data = MessageBoxPopup.CreateData(e.Message, _view.CharacterNameIF.text);
                _uiManager.OpenModal<MessageBoxPopup>(data);
            }

            finally
            {
                _uiManager.CloseModal<AwaitServerResponsePopup>();
            }
        }


        private void ClickOnBack()
        {
            _uiManager.BackToPreviousScreen();
        }
    }
}
