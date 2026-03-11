using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;
using System.Collections.Generic;


namespace Assets.Code.Common.StaticData
{
    public sealed class PlanetsRepository
    {
        private readonly IDataBaseManager _dataBase;

        public PlanetsRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }

        internal IReadOnlyList<PlanetORM> GetPlanets(string starSystemName)
        {
            return _dataBase.Query<PlanetORM>(
                "SELECT p.* FROM Planets p JOIN StarSystems s ON p.StarSystemId = s.Id WHERE s.Name = ?",
                starSystemName);
        }
    }
}
