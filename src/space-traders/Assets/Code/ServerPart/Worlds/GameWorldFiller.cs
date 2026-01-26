using Assets.Code.ClientPart.Gameplay.Features;
using Assets.Code.ServerPart.Gameplay.Features.PointsOfInteres.Factories;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ServerPart.Worlds
{
    internal sealed class GameWorldFiller
    {
        private readonly FeaturesContainer _featuresContainer;
        private readonly SpaceStationsFactory _spaceStationsFactory;

        public GameWorldFiller(FeaturesContainer featuresContainer,
            SpaceStationsFactory spaceStationsFactory)
        {
            _featuresContainer = featuresContainer;

            Debug.Log("### FeaturesContainer: " + _featuresContainer == null);
            _spaceStationsFactory = spaceStationsFactory;
        }

        public void FillWorld(string sceneName, Contexts contexts)
        {
            FillWithStations(sceneName, contexts);
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
