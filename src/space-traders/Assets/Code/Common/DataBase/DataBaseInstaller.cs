using Assets.Code.Common.DataBase.ORM;
using Assets.Code.Infrastructure.Loading;
using SQLite;
using System.Collections.Generic;


namespace Assets.Code.Common.DataBase
{
    internal sealed class DataBaseInstaller
    {
        internal void InstallDataBase(string dbPath)
        {
            var db = new SQLiteConnection(dbPath);

            db.CreateTable<PlayerORM>();
            db.CreateTable<CharacterORM>();
            db.CreateTable<CharacterShipORM>();
            db.CreateTable<CharacterLocationORM>();
            db.CreateTable<WalletORM>();
            db.CreateTable<ItemStackORM>();

            db.CreateTable<BuyOrderORM>();
            db.CreateTable<SellOrderORM>();

            db.CreateTable<StarSystemORM>();
            db.CreateTable<PlanetORM>();
            db.CreateTable<SpaceStationORM>();

            db.InsertAll(GetStarSystems());
            db.InsertAll(GetPlanets());
            db.InsertAll(GetStations());

            db.ExecuteScalar<string>("PRAGMA journal_mode=WAL;");
            db.Close();
        }

        private IEnumerable<StarSystemORM> GetStarSystems()
        {
            yield return new StarSystemORM
            {
                Name = "Sol",
                PositionX = 0,
                PositionY = 0,

                SceneName = SceneNames.GameScene1,
                Skybox = 0,
                LightSettings = 0,
            };

            yield return new StarSystemORM
            {
                Name = "Aldebaran",
                PositionX = 100,
                PositionY = 100,

                SceneName = SceneNames.GameScene1,
                Skybox = 1,
                LightSettings = 1,
            };
        }

        private IEnumerable<PlanetORM> GetPlanets()
        {
            yield return new PlanetORM
            {
                Name = "Cerberus",
                StarSystemId = 1,
                PositionX = 58_000_000,
                PositionY = 32_000_000,
                PlanetRadius = 6371,
                PlanetType = PlanetType.Acid,

                PrefabName = "Planet",
            };

            yield return new PlanetORM
            {
                Name = "DomProm",
                StarSystemId = 1,
                PositionX = 135_000_000,
                PositionY = -27_000_000,
                PlanetRadius = 6800,
                PlanetType = PlanetType.Desert,

                PrefabName = "Planet",
            };

            yield return new PlanetORM
            {
                Name = "Taurus",
                StarSystemId = 1,
                PositionX = 195_000_000,
                PositionY = 15_000_000,
                PlanetRadius = 10200,
                PlanetType = PlanetType.Carbon,

                PrefabName = "Planet",
            };

            yield return new PlanetORM
            {
                Name = "Pejnya",
                StarSystemId = 1,
                PositionX = 260_000_000,
                PositionY = -44_000_000,
                PlanetRadius = 8500,
                PlanetType = PlanetType.Water,

                PrefabName = "Planet",
            };

            yield return new PlanetORM
            {
                Name = "Ktyar Larm",
                StarSystemId = 1,
                PositionX = 470_000_000,
                PositionY = 38_000_000,
                PlanetRadius = 62000,
                PlanetType = PlanetType.GasGiant,

                PrefabName = "Planet",
            };

            yield return new PlanetORM
            {
                Name = "Fresno",
                StarSystemId = 1,
                PositionX = 540_000_000,
                PositionY = -52_000_000,
                PlanetRadius = 71000,
                PlanetType = PlanetType.GasGiant,

                PrefabName = "Planet",
            };
        }

        private IEnumerable<SpaceStationORM> GetStations()
        {
            yield return new SpaceStationORM
            {
                Name = "SpaceStation1",
                StarSystemId = 1,
                StationType = 0,
                PositionX = 0,
                PositionY = 0,

                PrefabName = "Stations/Station1"
            };

            yield return new SpaceStationORM
            {
                Name = "SpaceStation2",
                StarSystemId = 1,
                StationType = 1,
                PositionX = 0,
                PositionY = 250_000,

                PrefabName = "Stations/Station1"
            };

            yield return new SpaceStationORM
            {
                Name = "SpaceStation3",
                StarSystemId = 2,
                StationType = 0,
                PositionX = 0,
                PositionY = 0,

                PrefabName = "Stations/Station1"
            };

            yield return new SpaceStationORM
            {
                Name = "SpaceStation4",
                StarSystemId = 2,
                StationType = 1,
                PositionX = 0,
                PositionY = 250_000,

                PrefabName = "Stations/Station1"
            };
        }
    }
}
