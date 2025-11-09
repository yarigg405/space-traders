namespace Assets.Code.ClientPart.View.Factory
{
    public interface IEntityViewFactory
    {
        EntityBehaviour CreateViewForEntityFromPath(GameEntity entity);
        EntityBehaviour CreateViewForEntityFromPrefab(GameEntity entity);
    }
}
