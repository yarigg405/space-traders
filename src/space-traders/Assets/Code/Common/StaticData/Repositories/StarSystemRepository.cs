using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;
using System.Collections.Generic;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class StarSystemRepository
    {
        private readonly IDataBaseManager _dataBase;

        public StarSystemRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }

        internal IReadOnlyList<StarSystemORM> GetAll()
        {
            return _dataBase.Query<StarSystemORM>(
                "SELECT * FROM StarSystems");
        }

        internal StarSystemORM Get(int id)
        {
            return _dataBase.QuerySingle<StarSystemORM>(
                "SELECT * FROM StarSystems WHERE id=?", id);
        }
    }
}
