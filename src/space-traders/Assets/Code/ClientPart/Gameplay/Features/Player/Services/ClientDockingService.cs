using Assets.Code.ClientPart.Networking;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using Assets.Code.Networking;
using Cysharp.Threading.Tasks;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Services
{
    public sealed class ClientDockingService
    {
        private readonly ClientMessenger _messenger;
        private readonly IStateMachine _stateMachine;
        private readonly ILifetimeCancellationToken _lCts;

        private const int _dockAwaitDelay = 3500;

        internal ClientDockingService(ClientMessenger messenger,
            IStateMachine stateMachine,
            ILifetimeCancellationToken lCts)
        {
            _messenger = messenger;
            _stateMachine = stateMachine;
            _lCts = lCts;
        }

        internal void RequestDockTo(int stationId, int dockingBayIndex)
        {
            RequestDockToAsync(stationId, dockingBayIndex).Forget();
        }

        private async UniTask RequestDockToAsync(int stationId, int dockingBayIndex)
        {
            var ct = _lCts.Token;
            var result = await _messenger.RequestForDock(stationId, dockingBayIndex, ct);

            if (result.Equals("ok"))
            {
                await UniTask.Delay(_dockAwaitDelay);
                if (ct.IsCancellationRequested) return;

                _stateMachine.Enter<SpaceStationState>();
            }
        }
    }
}
