using Assets.Code.Infrastructure.Loading;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Networking;
using Assets.Code.UI;
using Assets.Code.UI.Screens;
using Assets.Code.UI.Screens.MessageBox;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;


namespace Assets.Code.ClientPart.Networking
{
    public sealed class GameEnterManager : IDisposable
    {
        private readonly IStateMachine _stateMachine;
        private readonly ClientMessenger _messenger;
        private readonly IUIManager _uiManager;
        private readonly AuthentificationContainer _authentification;

        private CancellationTokenSource _cts;

        internal GameEnterManager(IStateMachine stateMachine,
           ClientMessenger messenger, IUIManager uiManager, 
           AuthentificationContainer authentification)
        {
            _stateMachine = stateMachine;
            _messenger = messenger;
            _uiManager = uiManager;
            _authentification = authentification;
        }

        internal void EnterGame(int characterId)
        {
            _authentification.SelectedCharacterId = characterId;
            _cts = new();
            EnterAsync(characterId).Forget();
        }
        private async UniTask EnterAsync(int selectedCharacterId)
        {
            _uiManager.OpenModal<AwaitServerResponsePopup>();
            var token = _cts.Token;

            try
            {
                var sceneName = await _messenger.RequestForEnterTheGame(_authentification.SelectedCharacterId, token);
                token.ThrowIfCancellationRequested();
                HandleSceneEnter(sceneName);
            }

            catch (RequestFailedException e)
            {
                _uiManager.OpenModal<MessageBoxPopup>(e.Message);

                _cts?.Cancel();
                _cts?.Dispose();
            }

            finally
            {
                _uiManager.CloseModal<AwaitServerResponsePopup>();
            }
        }

        private void HandleSceneEnter(string sceneName)
        {
            if (sceneName.Equals(SceneNames.StationScene))
            {
                _stateMachine.Enter<SpaceStationState>();
            }

            else
            {
                _stateMachine.Enter<LoadGameSceneState, string>(sceneName);
            }
        }

        void IDisposable.Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
