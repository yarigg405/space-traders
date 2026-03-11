using System.Collections.Generic;

namespace Assets.Code.Common.DataBase
{
    public interface IDataBaseManager
    {
        IReadOnlyList<T> Query<T>(string sql, params object[] args) where T : new();
    }
}