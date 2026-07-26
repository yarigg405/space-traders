using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;
using System.Collections.Generic;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class SpaceStationsRepository
    {
        private readonly IDataBaseManager _dataBase;

        public SpaceStationsRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }

        internal IReadOnlyList<SpaceStationORM> GetStations(string starSystemName)
        {
            return _dataBase.Query<SpaceStationORM>(
                "SELECT st.* FROM SpaceStations st JOIN StarSystems s ON st.StarSystemId = s.Id WHERE s.Name = ?",
                starSystemName);
        }

        internal IReadOnlyList<SpaceStationORM> GetAll()
        {
            return _dataBase.Query<SpaceStationORM>("SELECT * FROM SpaceStations");
        }

        internal IReadOnlyList<SpaceStationORM> GetByStarSystemId(int starSystemId)
        {
            return _dataBase.Query<SpaceStationORM>(
                "SELECT * FROM SpaceStations WHERE starSystemId = ?", starSystemId);
        }

        internal SpaceStationORM GetById(int stationId)
        {
            return _dataBase.QuerySingle<SpaceStationORM>(
            "SELECT * FROM SpaceStations WHERE id = ?"
            , stationId);
        }
    }
}
