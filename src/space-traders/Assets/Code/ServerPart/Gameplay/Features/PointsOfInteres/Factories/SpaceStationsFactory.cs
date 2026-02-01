using Assets.Code.ClientPart.AssetManagement;
using Assets.Code.Common;
using Assets.Code.Common.Extensions;
using Assets.Code.Common.Physics;
using Assets.Code.Common.Physics.Registrars;
using Assets.Code.Infrastructure.Identifiers;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Gameplay.Features.PointsOfInteres.Factories
{
    public sealed class SpaceStationsFactory
    {
        private readonly IIdentifierService _identifier;
        private readonly IAssetProvider _assetProvider;

        public SpaceStationsFactory(IIdentifierService identifier, IAssetProvider assetProvider)
        {
            _identifier = identifier;
            _assetProvider = assetProvider;
        }

        public GameEntity CreateSpaceStation(double2 at, Contexts contexts)
        {
            var stationPrefabName = "Prefabs/Station1";

            var entity =
                CreateEntity.Empty(contexts)
                .AddId(_identifier.Next())
                .With(x => x.isStation = true)

                .AddViewPath(stationPrefabName)
                .AddGlobalPosition(at)
                .AddCurrentRotationY(0)
                ;

            var stationPrefab = _assetProvider.LoadAsset<SpaceStationRegistrar>(stationPrefabName);
            var childrenColliders = new ChildCollider[stationPrefab.DockingBays.Length];

            for (int i = 0; i < stationPrefab.DockingBays.Length; i++)
            {
                var dockingBay = stationPrefab.DockingBays[i];
                var deltaPos = dockingBay.transform.parent.localPosition;
                childrenColliders[i] = new ChildCollider(
                    at + deltaPos.ToDoble2XZ(),
                    dockingBay.Radius, i
                    );
            }
            entity.AddChildrenColliders(childrenColliders);

            return entity;
        }
    }
}
