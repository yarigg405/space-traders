using Assets.Code.Common;
using Unity.Mathematics;


namespace Assets.Code.ClientPart.Gameplay.Features.Navigation
{
    public static class NavigationTargetExtensions
    {
        public static bool IsNavigable(this GameEntity entity)
        {
            if (entity == null) return false;
            return (entity.isStation || entity.isPlanet) && HasCoordinate(entity);
        }

        public static bool HasCoordinate(this GameEntity entity)
        {
            return entity != null && (entity.hasGlobalPosition || entity.hasSkyboxCoordinates);
        }

        public static bool TryGetCoordinate(this GameEntity entity, out double2 coordinate)
        {
            if (entity == null)
            {
                coordinate = default;
                return false;
            }

            if (entity.hasGlobalPosition)
            {
                coordinate = entity.GlobalPosition;
                return true;
            }

            if (entity.hasSkyboxCoordinates)
            {
                coordinate = entity.SkyboxCoordinates;
                return true;
            }

            coordinate = default;
            return false;
        }

        public static string GetName(this GameEntity entity)
        {
            if (entity == null) return string.Empty;

            var type = entity.isStation ? "Station"
                : entity.isPlanet ? "Planet"
                : "Object";

            var id = entity.hasDatabaseId ? entity.DatabaseId : (int)entity.Id;
            return $"{type} #{id}";
        }

        public static string GetSpaceObjectType(this GameEntity entity)
        {
            if (entity == null) return string.Empty;

            var type = entity.isStation ? "Station"
                : entity.isPlanet ? "Planet"
                : "Object";

            return type;
        }

        public static bool TryGetUiDistance(this GameEntity entity, double2 from, out double distance)
        {
            if (entity.TryGetCoordinate(out var coordinate))
            {
                distance = math.distance(from, coordinate) * GameConstants.DISTANCE_REAL_TO_UI;
                return true;
            }

            distance = 0;
            return false;
        }
    }
}
