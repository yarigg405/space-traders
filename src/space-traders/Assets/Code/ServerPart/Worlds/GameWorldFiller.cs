using Assets.Code.ServerPart.Gameplay.Features.PointsOfInteres.Factories;
using Assets.Code.ServerPart.Gameplay.Features.SkyboxObjects.Factory;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Worlds
{
    internal sealed class GameWorldFiller
    {
        private readonly SpaceStationsFactory _spaceStationsFactory;
        private readonly SkyboxObjectFactory _skyboxFactory;

        public GameWorldFiller(SpaceStationsFactory spaceStationsFactory, SkyboxObjectFactory skyboxFactory)
        {
            _spaceStationsFactory = spaceStationsFactory;
            _skyboxFactory = skyboxFactory;
        }

        public void FillWorld(string sceneName, Contexts contexts)
        {
            FillWithPlanets(sceneName, contexts);
            FillWithStations(sceneName, contexts);
        }

        private void FillWithPlanets(string sceneName, Contexts contexts)
        {
            _skyboxFactory.CreatePlanet(new(0, 5_795_500_000), "Planet", contexts);
            _skyboxFactory.CreatePlanet(new(25000, 25000), "Planet", contexts);
            // _skyboxFactory.CreatePlanet(new double2 
        }

        private void FillWithStations(string sceneName, Contexts contexts)
        {
            _spaceStationsFactory.CreateSpaceStation(double2.zero, contexts);
            _spaceStationsFactory.CreateSpaceStation(new double2(0, 25_000), contexts);
            _spaceStationsFactory.CreateSpaceStation(new double2(0, 250_000), contexts);
            _spaceStationsFactory.CreateSpaceStation(new double2(0, 2_500_000), contexts);
            _spaceStationsFactory.CreateSpaceStation(new double2(0, 25_000_000), contexts);
            _spaceStationsFactory.CreateSpaceStation(new double2(0, 250_000_000), contexts);
        }
    }
}
