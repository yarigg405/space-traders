using Assets.Code.Common;
using Assets.Code.Common.Extensions;
using Assets.Code.Infrastructure.Identifiers;
using Assets.Code.ServerPart.Physics.Data;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Gameplay.Features.PointsOfInteres.Factories
{
    public sealed class SpaceStationsFactory
    {
        private readonly IIdentifierService _identifier;
        private readonly IPhysicsShapesProvider _physicsShapesProvider;

        public SpaceStationsFactory(IIdentifierService identifier, IPhysicsShapesProvider physicsShapesProvider)
        {
            _identifier = identifier;
            _physicsShapesProvider = physicsShapesProvider;
        }

        public GameEntity CreateSpaceStation(double2 at, Contexts contexts)
        {
            var stationPrefabName = "Prefabs/Stations/Station1";

            var entity =
                CreateEntity.Empty(contexts)
                .AddId(_identifier.Next())
                .With(x => x.isStation = true)

                .AddViewPath(stationPrefabName)
                .AddGlobalPosition(at)
                .AddCurrentRotationY(0)

                .With(x => x.isNeedInit = true)
                ;

            //var stationPrefab = _assetProvider.LoadAsset<SpaceStationRegistrar>(stationPrefabName);
            //var childrenColliders = new ChildCollider[stationPrefab.DockingBays.Length];

            //for (int i = 0; i < stationPrefab.DockingBays.Length; i++)
            //{
            //    var dockingBay = stationPrefab.DockingBays[i];
            //    var deltaPos = dockingBay.transform.parent.localPosition;
            //    childrenColliders[i] = new ChildCollider(
            //        at + deltaPos.ToDoble2XZ(),
            //        dockingBay.Radius, i
            //        );
            //}
            //entity.AddChildrenColliders(childrenColliders);

            return entity;
        }
    }
}
