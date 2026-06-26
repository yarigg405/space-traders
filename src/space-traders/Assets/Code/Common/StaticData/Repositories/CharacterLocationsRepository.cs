using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class CharacterLocationsRepository
    {
        private readonly IDataBaseManager _dataBase;

        public CharacterLocationsRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }

        internal CharacterLocationORM GetLocationForCharacter(int characterId)
        {
            return _dataBase.QuerySingle<CharacterLocationORM>(
                "SELECT * FROM CharacterLocations WHERE characterId =?"
                , characterId);
        }

        internal void UpdateLocation(CharacterLocationORM location)
        {
            _dataBase.Update(location);
        }
    }
}
