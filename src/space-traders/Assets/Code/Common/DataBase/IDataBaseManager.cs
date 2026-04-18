using System.Collections.Generic;


namespace Assets.Code.Common.DataBase
{
    public interface IDataBaseManager
    {
        IReadOnlyList<T> Query<T>(string sql, params object[] args) where T : new();
        T QuerySingle<T>(string sql, params object[] args) where T : new ();

        int CreateNew(object newObject);

        int Update(object updatedObject);

        void Execute(string sql, params object[] args);
    }
}