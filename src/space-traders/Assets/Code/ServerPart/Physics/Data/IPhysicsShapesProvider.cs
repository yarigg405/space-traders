namespace Assets.Code.ServerPart.Physics.Data
{
    public interface IPhysicsShapesProvider
    {
        PhysicsShape[] GetShapeForPrefab(string prefabName);
    }
}