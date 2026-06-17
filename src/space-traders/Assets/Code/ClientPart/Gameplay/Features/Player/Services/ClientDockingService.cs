using Assets.Code.ClientPart.Networking;
using Assets.Code.Infrastructure.States.GameStates;
using Assets.Code.Infrastructure.States.StateMachine;
using Cysharp.Threading.Tasks;
using System.Threading;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Services
{
    public sealed class ClientDockingService
    {
        private readonly ClientMessenger _messenger;
        private readonly IStateMachine _stateMachine;

        private CancellationTokenSource _cts = new();

        internal ClientDockingService(ClientMessenger messenger,
            IStateMachine stateMachine)
        {
            _messenger = messenger;
            _stateMachine = stateMachine;
        }

        internal void RequestDockTo(int stationId, int dockingBayIndex)
        {
            RequestDockToAsync(stationId, dockingBayIndex).Forget();
        }

        private async UniTask RequestDockToAsync(int stationId, int dockingBayIndex)
        {
            var result = await _messenger.RequestForDock(stationId, dockingBayIndex, _cts.Token);

            if (result.Equals("ok"))
            {
                await UniTask.Delay(1000);
                _stateMachine.Enter<SpaceStationState>();
            }
        }
    }
}
