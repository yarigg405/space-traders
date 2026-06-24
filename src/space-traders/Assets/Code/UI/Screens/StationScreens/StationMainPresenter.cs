using Assets.Code.ClientPart.Networking;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Networking.Data;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.UI.Infrastructure.Interfaces;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;


namespace Assets.Code.UI.Screens.StationScreens
{
    public sealed class StationMainPresenter : IPresenter<StationMainView>
    {
        private readonly IUIManager _uiManager;
        private readonly ClientMessenger _messenger;
        private readonly IStateMachine _stateMachine;

        private CancellationTokenSource _cts;

        private StationMainView _view;

        internal StationMainPresenter(IUIManager uiManager, 
            ClientMessenger messenger, IStateMachine stateMachine)
        {
            _uiManager = uiManager;
            _messenger = messenger;
            _stateMachine = stateMachine;
        }

        void IPresenter<StationMainView>.Show(StationMainView view, object args)
        {
            if (args is not LoadStationData data)
            {
                Debug.LogError("StationScreenData is need to open screen");
                return;
            }

            _uiManager.ClearHistory();
            _view = view;

            view.StationNameTmp.text = data.StationName;
            view.UndockButton.onClick.AddListener(ClickUndock);

            _cts = new();
        }

        void IPresenter<StationMainView>.Hide(StationMainView view)
        {
            view.UndockButton.onClick.RemoveListener(ClickUndock);
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private void ClickUndock()
        {
            UndockShipAsync(_cts.Token).Forget();
        }

        private async UniTask UndockShipAsync(CancellationToken ct)
        {
            var sceneData = await _messenger.RequestForUndock(ct);
            ct.ThrowIfCancellationRequested();
            _stateMachine.Enter<LoadGameSceneState, SpaceSceneData>(sceneData);
        }
    }

    public struct LoadStationData
    {
        public int StationId;
        public string StationName;        
    }
}
