using Assets.Code.Common;
using Assets.Code.Common.Extensions;
using Assets.Code.Infrastructure.Identifiers;
using Assets.Code.ServerPart.Physics.Data;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Gameplay.Features.PointsOfInterest.Factories
{
    public sealed class SpaceStationsFactory
    {
        private readonly IIdentifierService _identifier;
        private readonly IPhysicsShapesProvider _physicsShapesProvider;

        public SpaceStationsFactory(IIdentifierService identifier,
            IPhysicsShapesProvider physicsShapesProvider)
        {
            _identifier = identifier;
            _physicsShapesProvider = physicsShapesProvider;
        }

        public GameEntity CreateSpaceStation(int databaseId, double2 at, string prefabName, Contexts contexts)
        {
            var entity =
                CreateEntity.Empty(contexts)
                .AddId(_identifier.Next())
                .With(x => x.isStation = true)
                .AddDatabaseId(databaseId)

                .AddViewPath("Prefabs/" + prefabName)
                .AddGlobalPosition(at)
                .AddCurrentRotationY(0)
                .AddChildrenEntities(new())
                .AddCollectCollisionsInterval(1f)
                .AddCollectCollisionsTimer(0f)
                ;

            var triggers = _physicsShapesProvider.GetShapeForPrefab(prefabName);
            for (int i = 0; i < triggers.Length; i++)
            {
                PhysicsShape trigger = triggers[i];
                var triggerEntity = CreateEntity.Empty(contexts)
                      .AddId(_identifier.Next())
                      .AddParentEntity(entity.Id)
                      .With(x => x.isTrigger = true)
                      .With(x => x.isStationDockingBay = true)
                      .AddStationDockingBayIndex(i)
                      .AddPhysicsRadius(trigger.Radius)
                      .AddGlobalPosition(at + trigger.LocalCenter)
                      .AddCollisionsBuffer(new(4))
                      .AddTriggerEnterEventHandler(new(4))
                      .AddTriggerExitEventHandler(new(4))
                      .AddShipsInDockZone(new(4))
                      ;

                entity.ChildrenEntities.Add(triggerEntity);
            }

            return entity;
        }
    }
}
