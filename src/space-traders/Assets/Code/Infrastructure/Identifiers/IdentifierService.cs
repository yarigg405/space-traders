namespace Assets.Code.Infrastructure.Identifiers
{
    public sealed class IdentifierService : IIdentifierService
    {
        private ulong _lastId = 1;
        public ulong Next() =>
            ++_lastId;
    }
}
