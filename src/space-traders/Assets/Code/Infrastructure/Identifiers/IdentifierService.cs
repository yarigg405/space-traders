namespace Assets.Code.Infrastructure.Identifiers
{
    internal sealed class IdentifierService : IIdentifierService
    {
        private int _lastId = 1;
        public int Next() =>
            ++_lastId;
    }
}
