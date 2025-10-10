namespace Assets.Code.Infrastructure.Identifiers
{
    public sealed class IdentifierService : IIdentifierService
    {
        private int _lastId = 1;
        public int Next() =>
            ++_lastId;
    }
}
