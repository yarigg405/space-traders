using System;
using System.Threading;


namespace Assets.Code.Networking
{
    public sealed class LifetimeCancellationToken : ILifetimeCancellationToken, IDisposable
    {
        CancellationToken ILifetimeCancellationToken.Token => _cts.Token;
        private readonly CancellationTokenSource _cts = new();

        void IDisposable.Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
