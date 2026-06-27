using Assets.Code.ClientPart.Networking;
using Assets.Code.ClientPart.View;
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
        private readonly StationSceneDataHolder _stationSceneDataHolder;
        private readonly StationVisualApplier _stationVisualApplier;

        private readonly CancellationTokenSource _cts = new();

        public SpaceStationSceneEntryPoint(IUIManager uiManager,
            IScenesLoader scenesLoader, ClientMessenger messenger,
            AuthentificationContainer authentification,
            StationSceneDataHolder stationSceneDataHolder,
            StationVisualApplier stationVisualApplier)
        {
            _uiManager = uiManager;
            _scenesLoader = scenesLoader;
            _messenger = messenger;
            _authentification = authentification;
            _stationSceneDataHolder = stationSceneDataHolder;
            _stationVisualApplier = stationVisualApplier;
        }

        void IStartable.Start()
        {
            LoadSceneData(_cts.Token).Forget();
        }

        private async UniTask LoadSceneData(CancellationToken ct)
        {
            var data = await _messenger.RequestForLoadStation(_authentification.SelectedCharacterId, ct);
            ct.ThrowIfCancellationRequested();

            _stationSceneDataHolder.Current = data;
            _stationVisualApplier.Apply(data.StationType);
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
