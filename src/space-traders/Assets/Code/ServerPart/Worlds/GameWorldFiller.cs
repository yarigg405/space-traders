using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.ServerPart.Gameplay.Features.PointsOfInterest.Factories;
using Assets.Code.ServerPart.Gameplay.Features.SkyboxObjects.Factory;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Worlds
{
    internal sealed class GameWorldFiller
    {
        private readonly SpaceStationsFactory _spaceStationsFactory;
        private readonly SkyboxObjectFactory _skyboxFactory;

        private readonly PlanetsRepository _planetsRepository;
        private readonly SpaceStationsRepository _spaceStationsRepository;

        public GameWorldFiller(SpaceStationsFactory spaceStationsFactory,
            SkyboxObjectFactory skyboxFactory, PlanetsRepository planetsRepository,
            SpaceStationsRepository spaceStationsRepository)
        {
            _spaceStationsFactory = spaceStationsFactory;
            _skyboxFactory = skyboxFactory;
            _planetsRepository = planetsRepository;
            _spaceStationsRepository = spaceStationsRepository;
        }

        public void FillWorld(string sceneName, Contexts contexts)
        {
            FillWithPlanets(sceneName, contexts);
            FillWithStations(sceneName, contexts);
        }

        private void FillWithPlanets(string sceneName, Contexts contexts)
        {
            var planets = _planetsRepository.GetPlanets(sceneName);

            foreach (var planet in planets)
            {
                double2 pos = new(planet.PositionX, planet.PositionY);
                _skyboxFactory.CreatePlanet(pos, planet.PrefabName, contexts);
            }
        }

        private void FillWithStations(string sceneName, Contexts contexts)
        {
            var stations = _spaceStationsRepository.GetStations(sceneName);

            foreach (var station in stations)
            {
                double2 pos = new(station.PositionX, station.PositionY);
                _spaceStationsFactory.CreateSpaceStation(pos, station.PrefabName, contexts);
            }
        }
    }
}
