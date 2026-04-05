using System.Threading;


namespace Assets.Code.Networking
{
    public interface ICancellationToken
    {
        public CancellationToken Token { get; }
    }
}
