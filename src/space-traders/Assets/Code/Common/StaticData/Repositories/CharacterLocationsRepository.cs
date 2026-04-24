using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class CharacterLocationsRepository
    {
        private readonly IDataBaseManager _database;

        public CharacterLocationsRepository(IDataBaseManager database)
        {
            _database = database;
        }

        internal CharacterLocationORM GetLocationForCharacter(int characterId)
        {
            return _database.QuerySingle<CharacterLocationORM>(
                "SELECT * FROM CharacterLocations WHERE characterId =?"
                , characterId);
        }
    }
}
