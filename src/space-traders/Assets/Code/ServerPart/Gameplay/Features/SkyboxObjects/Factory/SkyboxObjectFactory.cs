using Assets.Code.Common;
using Assets.Code.Common.Extensions;
using Assets.Code.Infrastructure.Identifiers;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Gameplay.Features.SkyboxObjects.Factory
{
    public sealed class SkyboxObjectFactory
    {
        private readonly IIdentifierService _identifier;

        public SkyboxObjectFactory(IIdentifierService identifier)
        {
            _identifier = identifier;
        }

        internal GameEntity CreatePlanet(string name, double2 at, string prefabName, Contexts contexts)
        {
            return CreateEntity.Empty(contexts)
                .AddId(_identifier.Next())
                .AddViewPath("Prefabs/" + prefabName)
                .With(x => x.isPlanet = true)
                .AddDatabaseName(name)

                .AddSkyboxCoordinates(at)
                .AddQuadrantIndex(CalculateQuadrantIndex(at))
                ;
        }

        private int2 CalculateQuadrantIndex(double2 position)
        {
            var x = position.x;
            var y = position.y;

            int quadrantX = (int)math.floor((x + GameConstants.GAME_SCENE_HALF_QUADRANT_SIZE) / GameConstants.GAME_SCENE_QUADRANT_SIZE);
            int quadrantY = (int)math.floor((y + GameConstants.GAME_SCENE_HALF_QUADRANT_SIZE) / GameConstants.GAME_SCENE_QUADRANT_SIZE);

            return new(quadrantX, quadrantY);
        }
    }
}
