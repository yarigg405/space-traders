using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;


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
    }
}
