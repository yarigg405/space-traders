namespace Assets.Code.Infrastructure.Identifiers
{
    public sealed class IdentifierService : IIdentifierService
    {
        private uint _lastId = 1;
        public uint Next() =>
            ++_lastId;
    }
}
