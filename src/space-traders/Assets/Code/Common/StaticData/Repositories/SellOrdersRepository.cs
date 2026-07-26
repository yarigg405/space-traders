using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;
using System.Collections.Generic;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class SellOrdersRepository
    {
        public const int NpcOwnerId = 0;

        private readonly IDataBaseManager _dataBase;

        public SellOrdersRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }

        internal void Insert(SellOrderORM order)
        {
            _dataBase.CreateNew(order);
        }

        internal void DeleteAllNpcOrders()
        {
            _dataBase.Execute("DELETE FROM SellOrders WHERE sellerId = ?", NpcOwnerId);
        }

        internal IReadOnlyList<SellOrderORM> GetByStation(int stationId)
        {
            return _dataBase.Query<SellOrderORM>(
                "SELECT * FROM SellOrders WHERE stationId = ?", stationId);
        }
    }
}
