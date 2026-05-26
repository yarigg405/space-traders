using Assets.Code.Common.DataBase;
using Assets.Code.Common.DataBase.ORM;
using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.Networking;
using System;
using System.Linq;


namespace Assets.Code.Common.StaticData.Staff
{
    internal sealed class CharactersCreatingService
    {
        private readonly CharactersRepository _charactersRepository;
        private readonly StarSystemRepository _starSystemRepository;
        private readonly SpaceStationsRepository _spacesStationsRepository;

        private readonly IDataBaseManager _database;

        public CharactersCreatingService(CharactersRepository charactersRepository,
            IDataBaseManager database, StarSystemRepository starSystemRepository,
            SpaceStationsRepository spacesStationsRepository)
        {
            _charactersRepository = charactersRepository;
            _database = database;
            _starSystemRepository = starSystemRepository;
            _spacesStationsRepository = spacesStationsRepository;
        }

        internal bool TryCreateNewCharacter(int playerId, CharacterORM character, out string error)
        {
            if (character.Name.Length < 1)
            {
                error = ErrorCodes.EmptyCharacterName;
                return false;
            }

            var playerCharacters = _charactersRepository.GetCharactersForPlayer(playerId);
            if (playerCharacters.FirstOrDefault(x => x.Name.Equals(character.Name)) != null)
            {
                error = ErrorCodes.CharacterExists;
                return false;
            }


            try
            {
                var startSystemName = _starSystemRepository.GetAll().First().Name;

                _database.RunInTransaction(_database =>
                {
                    _database.CreateNew(character);
                    var characterId = character.Id;

                    var location = new CharacterLocationORM
                    {
                        CharacterId = characterId,
                        LocationType = LocationType.Station,
                        CurrentLocationId = _spacesStationsRepository.GetStations(startSystemName).First().Id,
                        DockBayId = 0
                    };
                    _database.CreateNew(location);

                });

                error = "";
                return true;
            }

            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

    }
}
