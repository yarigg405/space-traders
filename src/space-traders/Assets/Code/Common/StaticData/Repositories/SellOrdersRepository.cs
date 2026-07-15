using Assets.Code.Common.DataBase;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class SellOrdersRepository
    {
        private readonly IDataBaseManager _dataBase;

        public SellOrdersRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }
    }
}
