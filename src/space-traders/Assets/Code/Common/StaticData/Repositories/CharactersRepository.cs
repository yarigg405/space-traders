using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;
using System.Collections.Generic;
using System.Linq;


namespace Assets.Code.Common.StaticData.Repositories
{
    internal class CharactersRepository
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

        internal bool TryCreateNewCharacter(int playerId, CharacterORM character, out string error)
        {
            if (character.Name.Length < 1)
            {
                error = "error-empty-name";
                return false;
            }

            var playerCharacters = GetCharactersForPlayer(playerId);
            if (playerCharacters.FirstOrDefault(x => x.Name.Equals(character.Name)) != null)
            {
                error = "error-char-exists";
                return false;
            }


            _dataBase.CreateNew(character);

            error = "";
            return true;
        }
    }
}
