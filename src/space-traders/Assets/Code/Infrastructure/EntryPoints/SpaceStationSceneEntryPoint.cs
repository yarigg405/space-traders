using Assets.Code.ClientPart.Networking;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Networking;
using Assets.Code.UI;
using Assets.Code.UI.Screens;
using Assets.Code.UI.Screens.StationScreens;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using VContainer.Unity;


namespace Assets.Code.Infrastructure.EntryPoints
{
    internal sealed class SpaceStationSceneEntryPoint : IStartable, IDisposable
    {
        private readonly IUIManager _uiManager;
        private readonly IScenesLoader _scenesLoader;
        private readonly ClientMessenger _messenger;
        private readonly AuthentificationContainer _authentification;

        private readonly CancellationTokenSource _cts = new();

        public SpaceStationSceneEntryPoint(IUIManager uiManager,
            IScenesLoader scenesLoader, ClientMessenger messenger, 
            AuthentificationContainer authentification)
        {
            _uiManager = uiManager;
            _scenesLoader = scenesLoader;
            _messenger = messenger;
            _authentification = authentification;
        }

        void IStartable.Start()
        {
            LoadSceneData().Forget();
        }

        private async UniTask LoadSceneData()
        {
            var data = await _messenger.RequestForLoadStation(_authentification.SelectedCharacterId, _cts.Token);
            _uiManager.GoToScreen<StationMainScreen>(data);
            _uiManager.ClearHistory();

            _scenesLoader.SetSceneLoaded();
        }

        void IDisposable.Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
