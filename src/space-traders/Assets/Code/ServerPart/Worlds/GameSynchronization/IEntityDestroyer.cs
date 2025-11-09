namespace Assets.Code.ServerPart.Worlds.GameSynchronization
{
    public interface IEntityDestroyer
    {
        void DestroyEntityOnClients(uint entityId);
    }}
