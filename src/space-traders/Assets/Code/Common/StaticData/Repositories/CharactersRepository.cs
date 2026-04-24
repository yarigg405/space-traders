using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;
using System.Collections.Generic;


namespace Assets.Code.Common.StaticData.Repositories
{
    public sealed class CharactersRepository
    {
        private readonly IDataBaseManager _dataBase;

        public CharactersRepository(IDataBaseManager dataBase)
        {
            _dataBase = dataBase;
        }

        internal IReadOnlyList<CharacterORM> GetCharactersForPlayer(int playerId)
        {
            return _dataBase.Query<CharacterORM>(
                "SELECT * FROM Characters WHERE playerId = ?",
                playerId);
        }
        
    }
}
