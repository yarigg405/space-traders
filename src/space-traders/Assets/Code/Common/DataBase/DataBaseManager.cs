using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace Assets.Code.Common.DataBase
{
    public sealed class DataBaseManager : IDataBaseManager, IDisposable
    {
        private const string _databaseName = "DATA_BASE.db";
        private readonly string _dbPath = Path.Combine(Application.persistentDataPath, _databaseName);
        private readonly SQLiteConnection _connection;

        public DataBaseManager()
        {
            if (!File.Exists(_dbPath))
                CreateNewDataBase();

            _connection = new(_dbPath);
        }

        private void CreateNewDataBase()
        {
            var installer = new DataBaseInstaller();
            installer.InstallDataBase(_dbPath);
        }

        IReadOnlyList<T> IDataBaseManager.Query<T>(string sql, params object[] args)
        {
            return _connection.CreateCommand(sql, args).ExecuteQuery<T>();
        }

        T IDataBaseManager.QuerySingle<T>(string sql, params object[] args)
        {
            return _connection.CreateCommand(sql, args).ExecuteQuery<T>()
                .FirstOrDefault();
        }

        int IDataBaseManager.CreateNew(object newObject)
        {
            return _connection.Insert(newObject);
        }

        int IDataBaseManager.Update(object updatedObject)
        {
            return _connection.Update(updatedObject);
        }

        void IDataBaseManager.Execute(string sql, params object[] args)
        {
            _connection.CreateCommand(sql, args).ExecuteNonQuery();
        }

        void IDisposable.Dispose()
        {
            _connection.Dispose();
        }
    }
}
