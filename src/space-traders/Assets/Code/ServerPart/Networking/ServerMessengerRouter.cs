using Assets.Code.Common.DataBase.ORM;
using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.Common.StaticData.Staff;
using Assets.Code.Networking;
using Assets.Code.ServerPart.Gameplay.Features.InputInteraction;
using Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ServerPart.Gameplay.Features.Player.Services;
using Riptide;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Networking
{
    internal sealed class ServerMessengerRouter
    {
        private readonly NetworkManager _networkManager;
        private readonly ServerInputService _serverInputService;
        private readonly CharactersCreatingService _characterCreator;
        private readonly ClientSceneConnector _clientSceneConnector;
        private readonly PlayerLocationManager _playerLocationManager;
        private readonly PlayerCharacterManager _playerCharacterManager;
        private readonly ServerDockingService _dockingService;

        private readonly PlayersRepository _playersRepository;
        private readonly CharactersRepository _charactersRepository;
        private readonly CharacterLocationsRepository _characterLocationsRepository;
        private readonly StarSystemRepository _starsSystemRepository;
        private readonly SpaceStationsRepository _spaceStationsRepository;
        private readonly CharacterShipsRepository _characterShipsRepository;
        private readonly WalletsRepository _walletsRepository;
        private readonly BuyOrdersRepository _buyOrdersRepository;
        private readonly SellOrdersRepository _sellOrdersRepository;


        public ServerMessengerRouter(NetworkManager networkManager,
            PlayersRepository playersRepository,
            CharactersRepository charactersRepository,
            ServerInputService serverInputService,
            CharactersCreatingService characterCreator,
            CharacterLocationsRepository characterLocationsRepository,
            StarSystemRepository starsSystemRepository,
            ClientSceneConnector clientSceneConnector,
            SpaceStationsRepository spaceStationsRepository,
            CharacterShipsRepository characterShipsRepository,
            WalletsRepository walletsRepository,
            BuyOrdersRepository buyOrdersRepository,
            SellOrdersRepository sellOrdersRepository,
            PlayerLocationManager playerLocationManager,
            PlayerCharacterManager playerCharacterManager,
            ServerDockingService dockingService)
        {
            _networkManager = networkManager;
            _playersRepository = playersRepository;
            _charactersRepository = charactersRepository;
            _serverInputService = serverInputService;
            _characterCreator = characterCreator;
            _characterLocationsRepository = characterLocationsRepository;
            _starsSystemRepository = starsSystemRepository;
            _clientSceneConnector = clientSceneConnector;
            _spaceStationsRepository = spaceStationsRepository;
            _characterShipsRepository = characterShipsRepository;
            _walletsRepository = walletsRepository;
            _buyOrdersRepository = buyOrdersRepository;
            _sellOrdersRepository = sellOrdersRepository;
            _playerLocationManager = playerLocationManager;
            _playerCharacterManager = playerCharacterManager;
            _dockingService = dockingService;
        }

        internal void HandleRequestGetCharacters(ushort fromClientId, Message message)
        {
            var login = message.GetString();
            var password = message.GetString();
            var messageId = message.GetUInt();

            if (_networkManager.CheckServerPassword(password))
            {
                var player = _playersRepository.GetOrCreatePlayer(login);
                var characters = _charactersRepository.GetCharactersForPlayer(player.Id);

                var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseGetCharacters)
                    .AddUInt(messageId)
                    .AddInt(characters.Count);

                foreach (var character in characters)
                {
                    response.AddInt(character.Id);
                    response.AddString(character.Name);
                }

                _networkManager.Server.Send(response, fromClientId);
            }

            else
            {
                var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.RequestFailed)
                    .AddUInt(messageId)
                    .AddString(ErrorCodes.WrongPassword);

                _networkManager.Server.Send(response, fromClientId);
            }
        }

        internal void HandleRequestCreateCharacter(ushort fromClientId, Message message)
        {
            var login = message.GetString();
            var characterName = message.GetString();
            var messageId = message.GetUInt();

            var player = _playersRepository.GetOrCreatePlayer(login);

            var character = new CharacterORM
            {
                PlayerId = player.Id,
                Name = characterName,
                CurrentShipId = 0
            };

            if (_characterCreator.TryCreateNewCharacter(player.Id, character, out string error))
            {
                var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseCreateCharacter)
                    .AddUInt(messageId)
                    .AddBool(true);

                _networkManager.Server.Send(response, fromClientId);
            }

            else
            {
                var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.RequestFailed)
                    .AddUInt(messageId)
                    .AddString(error);

                _networkManager.Server.Send(response, fromClientId);
            }
        }

        internal void HandleRequestEnterTheGame(ushort fromClientId, Message message)
        {
            var characterId = message.GetInt();
            var messageId = message.GetUInt();

            _playerCharacterManager.SetCharacterForPlayer(fromClientId, characterId);
            _clientSceneConnector.ConnectPlayer(fromClientId);

            var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseEnterTheGame)
                .AddUInt(messageId);

            var isStation = _playerLocationManager.IsCharacterInStation(characterId);
            response.AddBool(isStation);

            if (!isStation)
            {
                var sceneData = _playerLocationManager.GetSpaceSceneData(characterId);
                response.AddString(sceneData.SceneName)
                        .AddString(sceneData.ConfigJson);
            }

            _networkManager.Server.Send(response, fromClientId);
        }

        internal void HandleRequestForStationSceneData(ushort fromClientId, Message message)
        {
            var characterId = message.GetInt();
            var messageId = message.GetUInt();
            var location = _characterLocationsRepository.GetLocationForCharacter(characterId);
            var station = _spaceStationsRepository.GetById(location.CurrentLocationId);
            var starSystem = _starsSystemRepository.GetById(station.StarSystemId);
            var ship = _characterShipsRepository.GetCurrentShip(characterId);

            var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseLoadStationData)
                .AddUInt(messageId)
                .AddInt(station.Id)
                .AddString(station.Name)
                .AddString(starSystem.Name)
                .AddInt(station.StationType)
                .AddString(ship.ShipModelId)
                .AddString(ship.ShipFitJson)
                ;

            _networkManager.Server.Send(response, fromClientId);
        }

        internal void HandleRequestForUndock(ushort fromClientId, Message message)
        {
            var characterId = _playerCharacterManager.GetCharacterIdForPlayer(fromClientId);
            var messageId = message.GetUInt();

            _playerLocationManager.SetUndocked(characterId);
            _clientSceneConnector.ConnectPlayer(fromClientId, true);

            var sceneData = _playerLocationManager.GetSpaceSceneData(characterId);
            var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseToUndock)
                .AddUInt(messageId)
                .AddString(sceneData.SceneName)
                .AddString(sceneData.ConfigJson)
                ;

            _networkManager.Server.Send(response, fromClientId);
        }

        internal void HandleRequestForDock(ushort fromClientId, Message message)
        {
            var characterId = _playerCharacterManager.GetCharacterIdForPlayer(fromClientId);
            var stationId = message.GetInt();
            var dockingBayId = message.GetInt();
            var messageId = message.GetUInt();

            if (_dockingService.CharacterCanDock(characterId, stationId))
            {
                _dockingService.StartDocking(characterId, stationId, dockingBayId, fromClientId);

                var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseToDock)
                .AddUInt(messageId)
                .AddString("ok")
                ;
                _networkManager.Server.Send(response, fromClientId);
            }

            else
            {
                var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.RequestFailed)
                    .AddUInt(messageId)
                    .AddString("error");

                _networkManager.Server.Send(response, fromClientId);
            }
        }

        internal void HandleRequestForMoney(ushort fromClientId, Message message)
        {
            var characterId = _playerCharacterManager.GetCharacterIdForPlayer(fromClientId);
            var messageId = message.GetUInt();

            var money = _walletsRepository.GetCharacterMoney(characterId);

            var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseMoney)
                .AddUInt(messageId)
                .AddLong(money);

            _networkManager.Server.Send(response, fromClientId);
        }

        internal void HandleRequestForStationTradeData(ushort fromClientId, Message message)
        {
            var stationId = message.GetInt();
            var messageId = message.GetUInt();

            var currentStation = _spaceStationsRepository.GetById(stationId);
            var stations = _spaceStationsRepository.GetByStarSystemId(currentStation.StarSystemId);

            var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseStationTradeData)
                .AddUInt(messageId)
                .AddInt(stationId)
                .AddInt(stations.Count);

            foreach (var station in stations)
            {
                response.AddInt(station.Id)
                        .AddString(station.Name);

                var buyOrders = _buyOrdersRepository.GetByStation(station.Id);
                response.AddInt(buyOrders.Count);
                foreach (var order in buyOrders)
                {
                    response.AddString(order.ItemId)
                            .AddLong(order.Price)
                            .AddInt(order.Quantity)
                            .AddLong(order.ExpiresAt);
                }

                var sellOrders = _sellOrdersRepository.GetByStation(station.Id);
                response.AddInt(sellOrders.Count);
                foreach (var order in sellOrders)
                {
                    response.AddString(order.ItemId)
                            .AddLong(order.Price)
                            .AddInt(order.Quantity)
                            .AddLong(order.ExpiresAt);
                }
            }

            _networkManager.Server.Send(response, fromClientId);
        }

        internal void HandleEntitiesLoading(ushort fromClientId, Message message)
        {
            _clientSceneConnector.FillWorldForClient(fromClientId);
        }



        internal void HandleClientTargetRotation(ushort fromClientId, Message message)
        {
            var targetRotation = message.GetFloat();
            _serverInputService.SetPlayerTargetRotation(fromClientId, targetRotation);
        }

        internal void HandleClientSpeedModifier(ushort fromClientId, Message message)
        {
            var speedModifier = message.GetFloat();
            _serverInputService.SetPlayerSpeedModifier(fromClientId, speedModifier);
        }

        internal void HandleKeepDistance(ushort fromClientId, Message message)
        {
            var targetId = message.GetUInt();
            var minMaxDistance = message.GetVector2();

            _serverInputService.SetPlayerKeepDistance(fromClientId, targetId, minMaxDistance);
        }

        internal void HandleSetOrbig(ushort fromClientId, Message message)
        {
            var targetId = message.GetUInt();
            var orbitRadius = message.GetFloat();

            _serverInputService.SetPlayerOrbitMoving(fromClientId, targetId, orbitRadius);
        }

        internal void HandleSetWarpTo(ushort fromClientId, Message message)
        {
            var x = message.GetDouble();
            var y = message.GetDouble();
            var coordinates = new double2(x, y);

            _serverInputService.SetPlayerWarpTo(fromClientId, coordinates);
        }
    }
}
