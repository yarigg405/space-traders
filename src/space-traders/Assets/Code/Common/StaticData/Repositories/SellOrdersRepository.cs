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

        internal IReadOnlyList<SellOrderORM> GetByItem(string itemId)
        {
            return _dataBase.Query<SellOrderORM>(
                "SELECT * FROM SellOrders WHERE itemId = ?", itemId);
        }

        internal SellOrderORM GetById(long id)
        {
            return _dataBase.QuerySingle<SellOrderORM>(
                "SELECT * FROM SellOrders WHERE id = ?", id);
        }

        internal void SetQuantity(long id, int quantity)
        {
            if (quantity <= 0)
                _dataBase.Execute("DELETE FROM SellOrders WHERE id = ?", id);
            else
                _dataBase.Execute("UPDATE SellOrders SET quantity = ? WHERE id = ?", quantity, id);
        }
    }
}
