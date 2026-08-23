using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class WalletsRepository
    {
        private readonly IDataBaseManager _dataBase;

        public WalletsRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }

        internal long GetCharacterMoney(int characterId)
        {
            var wallet = _dataBase.QuerySingle<WalletORM>(
                "SELECT * FROM Wallets WHERE ownerType = ? AND ownerId = ?",
                WalletOwnerType.Character, characterId);

            return wallet?.Money ?? 0;
        }

        internal void ChangeMoney(int characterId, long delta)
        {
            var wallet = _dataBase.QuerySingle<WalletORM>(
                "SELECT * FROM Wallets WHERE ownerType = ? AND ownerId = ?",
                WalletOwnerType.Character, characterId);

            if (wallet == null)
            {
                _dataBase.CreateNew(new WalletORM
                {
                    OwnerType = WalletOwnerType.Character,
                    OwnerId = characterId,
                    Money = delta,
                });
            }
            else
            {
                wallet.Money += delta;
                _dataBase.Update(wallet);
            }
        }
    }
}
