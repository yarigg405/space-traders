namespace Assets.Code.Gameplay.Worlds.GameSynchronization
{
    public interface IEntityDestroyer
    {
        void DestroyEntityOnClients(uint entityId);
    }}
