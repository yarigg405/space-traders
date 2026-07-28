using Assets.Code.Common;
using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Networking.Data;
using Assets.Code.ServerPart.Physics.Data;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerLocationManager
    {
        private readonly CharacterLocationsRepository _locationsRepository;
        private readonly SpaceStationsRepository _spaceStationsRepository;
        private readonly StarSystemRepository _starSystemRepository;
        private readonly IPhysicsShapesProvider _physicsShapesStorage;


        public PlayerLocationManager(CharacterLocationsRepository locationsRepository,
            SpaceStationsRepository spaceStationsRepository,
            IPhysicsShapesProvider physicsShapesStorage,
            StarSystemRepository starSystemRepository)
        {
            _locationsRepository = locationsRepository;
            _spaceStationsRepository = spaceStationsRepository;
            _physicsShapesStorage = physicsShapesStorage;
            _starSystemRepository = starSystemRepository;
        }

        internal string GetWorldKeyForCharacter(int characterId)
        {
            var location = _locationsRepository.GetLocationForCharacter(characterId);

            if (location.LocationType == Common.DataBase.ORM.LocationType.Station)
                return SceneNames.StationScene;

            var spaceSystemId = location.CurrentLocationId;
            var spaceSystem = _starSystemRepository.GetById(spaceSystemId);
            return spaceSystem.Name;
        }

        internal bool IsCharacterInStation(int characterId)
        {
            var location = _locationsRepository.GetLocationForCharacter(characterId);
            return location.LocationType == Common.DataBase.ORM.LocationType.Station;
        }

        internal SpaceSceneData GetSpaceSceneData(int characterId)
        {
            var location = _locationsRepository.GetLocationForCharacter(characterId);
            var starSystem = _starSystemRepository.GetById(location.CurrentLocationId);

            var config = new SpaceSceneConfig
            {
                StarSystem = starSystem.Name,
                Skybox = starSystem.Skybox,
                LightSettings = starSystem.LightSettings,
            };

            return new SpaceSceneData
            {
                SceneName = starSystem.SceneName,
                ConfigJson = JsonUtility.ToJson(config),
            };
        }

        internal double2 GetCoordinatesForSpawn(int characterId)
        {
            var location = _locationsRepository.GetLocationForCharacter(characterId);

            if (location.LocationType == Common.DataBase.ORM.LocationType.Space)
            {
                return new(location.PositionX, location.PositionY);
            }

            if (location.LocationType == Common.DataBase.ORM.LocationType.Station)
            {
                var station = _spaceStationsRepository.GetById(location.CurrentLocationId);
                var global = new double2(station.PositionX, station.PositionY);

                var shape = _physicsShapesStorage.GetShapeForPrefab(station.PrefabName);
                var dockId = location.DockBayId;
                var dock = shape[location.DockBayId];

                var spawnPos = new double2(
                    station.PositionX + dock.LocalCenter.x,
                    station.PositionY + dock.LocalCenter.y
                    );

                return spawnPos.GetRandomCoordinatesAroundPointZX(15f);
            }

            return double2.zero;
        }


        internal void SaveGlobalPosition(int characterId, double2 globalPosition)
        {
            var location = _locationsRepository.GetLocationForCharacter(characterId);

            location.LocationType = Common.DataBase.ORM.LocationType.Space;
            location.PositionX = globalPosition.x;
            location.PositionY = globalPosition.y;

            _locationsRepository.UpdateLocation(location);
        }

        internal void SetStationDocked(int characterId, int stationId, int dockingBayNum)
        {
            var location = _locationsRepository.GetLocationForCharacter(characterId);

            location.LocationType = Common.DataBase.ORM.LocationType.Station;
            location.CurrentLocationId = stationId;
            location.DockBayId = dockingBayNum;

            _locationsRepository.UpdateLocation(location);
        }

        internal void SetUndocked(int characterId)
        {
            var location = _locationsRepository.GetLocationForCharacter(characterId);

            if (location.LocationType != Common.DataBase.ORM.LocationType.Station)
                return;

            var station = _spaceStationsRepository.GetById(location.CurrentLocationId);
            var global = new double2(station.PositionX, station.PositionY);

            var shape = _physicsShapesStorage.GetShapeForPrefab(station.PrefabName);
            var dockId = location.DockBayId;
            var dock = shape[location.DockBayId];

            var globalPos = new double2(
                station.PositionX + dock.LocalCenter.x,
                station.PositionY + dock.LocalCenter.y
                );

            location.LocationType = Common.DataBase.ORM.LocationType.Space;
            location.CurrentLocationId = station.StarSystemId;
            location.PositionX = globalPos.x;
            location.PositionY = globalPos.y;

            _locationsRepository.UpdateLocation(location);
        }
    }
}
