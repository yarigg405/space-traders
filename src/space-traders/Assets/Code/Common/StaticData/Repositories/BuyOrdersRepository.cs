using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;
using System.Collections.Generic;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class BuyOrdersRepository
    {
        public const int NpcOwnerId = 0;

        private readonly IDataBaseManager _dataBase;

        public BuyOrdersRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }

        internal void Insert(BuyOrderORM order)
        {
            _dataBase.CreateNew(order);
        }

        internal void DeleteAllNpcOrders()
        {
            _dataBase.Execute("DELETE FROM BuyOrders WHERE sellerId = ?", NpcOwnerId);
        }

        internal IReadOnlyList<BuyOrderORM> GetByStation(int stationId)
        {
            return _dataBase.Query<BuyOrderORM>(
                "SELECT * FROM BuyOrders WHERE stationId = ?", stationId);
        }
    }
}
