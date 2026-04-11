using System.Threading;


namespace Assets.Code.Networking
{
    public interface ILifetimeCancellationToken
    {
        public CancellationToken Token { get; }
    }
}
