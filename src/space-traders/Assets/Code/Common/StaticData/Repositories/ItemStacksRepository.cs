using Assets.Code.Common.DataBase;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class ItemStacksRepository
    {
        private readonly IDataBaseManager _dataBase;

        public ItemStacksRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }
    }
}
