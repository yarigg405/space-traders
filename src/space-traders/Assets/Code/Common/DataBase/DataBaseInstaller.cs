using Assets.Code.Common.DataBase.ORM;
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
            db.CreateTable<PlayerShipORM>();
            db.CreateTable<CharacterLocationORM>();

            db.CreateTable<StarSystemORM>();
            db.CreateTable<PlanetORM>();
            db.CreateTable<SpaceStationORM>();

            db.InsertAll(GetStarSystems());
            db.InsertAll(GetPlanets());
            db.InsertAll(GetStations());

            db.Execute("PRAGMA journal_mode=WAL;");
            db.Close();
        }

        private IEnumerable<StarSystemORM> GetStarSystems()
        {
            yield return new StarSystemORM
            {
                Name = "GameScene1",
                PositionX = 0,
                PositionY = 0,
            };

            yield return new StarSystemORM
            {
                Name = "GameScene2",
                PositionX = 100,
                PositionY = 100,
            };
        }

        private IEnumerable<PlanetORM> GetPlanets()
        {
            yield return new PlanetORM
            {
                Name = "Planet1",
                StarSystemId = 1,
                PositionX = 0,
                PositionY = 5_795_500_000,

                PrefabName = "Planet",
            };

            yield return new PlanetORM
            {
                Name = "Planet2",
                StarSystemId = 1,
                PositionX = 25000,
                PositionY = 25000,

                PrefabName = "Planet",
            };

            yield return new PlanetORM
            {
                Name = "Planet3",
                StarSystemId = 2,
                PositionX = 0,
                PositionY = 5_795_500_000,

                PrefabName = "Planet",
            };

            yield return new PlanetORM
            {
                Name = "Planet4",
                StarSystemId = 2,
                PositionX = 25000,
                PositionY = 25000,

                PrefabName = "Planet",
            };
        }

        private IEnumerable<SpaceStationORM> GetStations()
        {
            yield return new SpaceStationORM
            {
                Name = "SpaceStation1",
                StarSystemId = 1,
                PositionX = 0,
                PositionY = 0,

                PrefabName = "Stations/Station1"
            };

            yield return new SpaceStationORM
            {
                Name = "SpaceStation2",
                StarSystemId = 1,
                PositionX = 0,
                PositionY = 250_000,

                PrefabName = "Stations/Station1"
            };

            yield return new SpaceStationORM
            {
                Name = "SpaceStation3",
                StarSystemId = 2,
                PositionX = 0,
                PositionY = 0,

                PrefabName = "Stations/Station1"
            };

            yield return new SpaceStationORM
            {
                Name = "SpaceStation4",
                StarSystemId = 2,
                PositionX = 0,
                PositionY = 250_000,

                PrefabName = "Stations/Station1"
            };
        }
    }
}
