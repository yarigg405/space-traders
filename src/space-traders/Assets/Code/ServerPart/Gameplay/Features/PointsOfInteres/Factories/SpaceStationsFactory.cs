using Assets.Code.Common;
using Assets.Code.Common.Extensions;
using Assets.Code.Infrastructure.Identifiers;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Gameplay.Features.PointsOfInteres.Factories
{
    public sealed class SpaceStationsFactory
    {
        private readonly IIdentifierService _identifier;

        public SpaceStationsFactory(IIdentifierService identifier)
        {
            _identifier = identifier;
        }

        public GameEntity CreateSpaceStation(double2 at, Contexts contexts)
        {
            return
                CreateEntity.Empty(contexts)
                .AddId(_identifier.Next())
                .With(x => x.isStation = true)

                .AddViewPath("Prefabs/Station1")
                .AddGlobalPosition(at)
                .AddCurrentRotationY(0)
                ;
        }
    }
}
