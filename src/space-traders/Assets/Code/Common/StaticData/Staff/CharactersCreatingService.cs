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
        private const long StartingMoney = 0;
        private const string StartingShipModelId = "TestShip1";

        private readonly CharactersRepository _charactersRepository;
        private readonly StarSystemRepository _starSystemRepository;
        private readonly SpaceStationsRepository _spaceStationsRepository;

        private readonly IDataBaseManager _dataBase;

        public CharactersCreatingService(CharactersRepository charactersRepository,
            IDataBaseManager database, StarSystemRepository starSystemRepository,
            SpaceStationsRepository spaceStationsRepository)
        {
            _charactersRepository = charactersRepository;
            _dataBase = database;
            _starSystemRepository = starSystemRepository;
            _spaceStationsRepository = spaceStationsRepository;
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

                _dataBase.RunInTransaction(_database =>
                {
                    _database.CreateNew(character);
                    var characterId = character.Id;

                    var location = new CharacterLocationORM
                    {
                        CharacterId = characterId,
                        LocationType = LocationType.Station,
                        CurrentLocationId = _spaceStationsRepository.GetStations(startSystemName).First().Id,
                        PositionX = 0,
                        PositionY = 0,
                        DockBayId = 0
                    };
                    _database.CreateNew(location);

                    var wallet = new WalletORM
                    {
                        OwnerType = WalletOwnerType.Character,
                        OwnerId = characterId,
                        Money = StartingMoney
                    };
                    _database.CreateNew(wallet);

                    var ship = new CharacterShipORM
                    {
                        OwnerCharacterId = characterId,
                        ShipModelId = StartingShipModelId,
                        ShipState = ShipState.Selected,
                        ShipFitJson = string.Empty
                    };
                    _database.CreateNew(ship);

                    character.CurrentShipId = ship.Id;
                    _database.Update(character);
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
