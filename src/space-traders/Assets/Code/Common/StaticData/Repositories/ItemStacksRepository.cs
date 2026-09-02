using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;
using System.Collections.Generic;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class ItemStacksRepository
    {
        private readonly IDataBaseManager _dataBase;

        public ItemStacksRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }

        internal void CreateStationStack(string itemId, int amount, int stationId, int ownerCharacterId)
        {
            _dataBase.CreateNew(new ItemStackORM
            {
                ItemId = itemId,
                Amount = amount,
                ContainerType = ContainerType.StationHangar,
                ContainerId = stationId,
                OwnerType = ItemStackOwnerType.Character,
                OwnerId = ownerCharacterId,
            });
        }

        internal IReadOnlyList<ItemStackORM> GetStationStacksByOwner(int characterId)
        {
            return _dataBase.Query<ItemStackORM>(
                "SELECT * FROM ItemStacks WHERE ownerType = ? AND ownerId = ? AND containerType = ?",
                (int)ItemStackOwnerType.Character, characterId, (int)ContainerType.StationHangar);
        }

        internal int GetOwnedAmount(int characterId, int stationId, string itemId)
        {
            var rows = _dataBase.Query<ItemStackORM>(
                "SELECT * FROM ItemStacks WHERE ownerType = ? AND ownerId = ? AND containerType = ? AND containerId = ? AND itemId = ?",
                (int)ItemStackOwnerType.Character, characterId, (int)ContainerType.StationHangar, stationId, itemId);

            var total = 0;
            foreach (var row in rows)
                total += row.Amount;

            return total;
        }

        internal void RemoveFromStation(int characterId, int stationId, string itemId, int quantity)
        {
            var rows = _dataBase.Query<ItemStackORM>(
                "SELECT * FROM ItemStacks WHERE ownerType = ? AND ownerId = ? AND containerType = ? AND containerId = ? AND itemId = ? ORDER BY id",
                (int)ItemStackOwnerType.Character, characterId, (int)ContainerType.StationHangar, stationId, itemId);

            var remaining = quantity;
            foreach (var row in rows)
            {
                if (remaining <= 0)
                    break;

                if (row.Amount <= remaining)
                {
                    remaining -= row.Amount;
                    _dataBase.Execute("DELETE FROM ItemStacks WHERE id = ?", row.Id);
                }
                else
                {
                    _dataBase.Execute("UPDATE ItemStacks SET amount = ? WHERE id = ?", row.Amount - remaining, row.Id);
                    remaining = 0;
                }
            }
        }
    }
}
