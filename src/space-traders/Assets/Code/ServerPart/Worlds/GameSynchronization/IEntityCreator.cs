namespace Assets.Code.ServerPart.Worlds.GameSynchronization
{
    public interface IEntityCreator
    {
        void CreateEntityOnClients(GameEntity entity);
    }
}
