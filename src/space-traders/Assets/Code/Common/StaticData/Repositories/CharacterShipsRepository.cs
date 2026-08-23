using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class CharacterShipsRepository
    {
        private readonly IDataBaseManager _dataBase;

        public CharacterShipsRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }

        internal CharacterShipORM GetCurrentShip(int characterId)
        {
            return _dataBase.QuerySingle<CharacterShipORM>(
                "SELECT s.* FROM PlayerShips s JOIN Characters c ON c.currentShipId = s.id WHERE c.id = ?",
                characterId);
        }
    }
}
