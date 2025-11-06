namespace Assets.Code.Gameplay.Worlds.GameSynchronization
{
    public interface IEntityCreator
    {
        void CreateEntityOnClients(GameEntity entity);
    }
}
