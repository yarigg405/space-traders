using Assets.Code.Common.DataBase;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class BuyOrdersRepository
    {
        private readonly IDataBaseManager _dataBase;

        public BuyOrdersRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }
    }
}
