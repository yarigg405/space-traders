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

        internal IReadOnlyList<BuyOrderORM> GetByItem(string itemId)
        {
            return _dataBase.Query<BuyOrderORM>(
                "SELECT * FROM BuyOrders WHERE itemId = ?", itemId);
        }

        internal BuyOrderORM GetById(long id)
        {
            return _dataBase.QuerySingle<BuyOrderORM>(
                "SELECT * FROM BuyOrders WHERE id = ?", id);
        }

        internal void SetQuantity(long id, int quantity)
        {
            if (quantity <= 0)
                _dataBase.Execute("DELETE FROM BuyOrders WHERE id = ?", id);
            else
                _dataBase.Execute("UPDATE BuyOrders SET quantity = ? WHERE id = ?", quantity, id);
        }
    }
}
