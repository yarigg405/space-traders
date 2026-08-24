using Assets.Code.Common.DataBase.ORM;
using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.Common.StaticData.Staff;
using Assets.Code.Networking;
using Assets.Code.ServerPart.Gameplay.Features.InputInteraction;
using Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ServerPart.Gameplay.Features.Player.Services;
using Assets.Code.ServerPart.Gameplay.Features.Trading;
using Riptide;
using System;
using System.Collections.Generic;
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
        private readonly PurchaseService _purchaseService;


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
            PurchaseService purchaseService,
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
            _purchaseService = purchaseService;
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

        internal void HandleRequestItemOrders(ushort fromClientId, Message message)
        {
            var itemId = message.GetString();
            var messageId = message.GetUInt();

            var characterId = _playerCharacterManager.GetCharacterIdForPlayer(fromClientId);
            var location = _characterLocationsRepository.GetLocationForCharacter(characterId);
            var currentStation = _spaceStationsRepository.GetById(location.CurrentLocationId);

            var buyOrders = _buyOrdersRepository.GetByItem(itemId);
            var sellOrders = _sellOrdersRepository.GetByItem(itemId);

            var buyByStation = new Dictionary<int, List<BuyOrderORM>>();
            foreach (var order in buyOrders)
                GetOrCreate(buyByStation, order.StationId).Add(order);

            var sellByStation = new Dictionary<int, List<SellOrderORM>>();
            foreach (var order in sellOrders)
                GetOrCreate(sellByStation, order.StationId).Add(order);

            var stationIds = new HashSet<int>();
            foreach (var id in buyByStation.Keys) stationIds.Add(id);
            foreach (var id in sellByStation.Keys) stationIds.Add(id);

            var systemNames = new Dictionary<int, string>();

            var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseItemOrders)
                .AddUInt(messageId)
                .AddInt(currentStation.Id)
                .AddInt(currentStation.StarSystemId)
                .AddDouble(currentStation.PositionX)
                .AddDouble(currentStation.PositionY)
                .AddInt(stationIds.Count);

            foreach (var stationId in stationIds)
            {
                var station = _spaceStationsRepository.GetById(stationId);

                response.AddInt(station.Id)
                        .AddString(station.Name)
                        .AddDouble(station.PositionX)
                        .AddDouble(station.PositionY)
                        .AddInt(station.StarSystemId)
                        .AddString(GetStarSystemName(station.StarSystemId, systemNames));

                buyByStation.TryGetValue(stationId, out var stationBuys);
                response.AddInt(stationBuys?.Count ?? 0);
                if (stationBuys != null)
                    foreach (var order in stationBuys)
                        response.AddLong(order.Id).AddString(order.ItemId).AddLong(order.Price)
                                .AddInt(order.Quantity).AddLong(order.ExpiresAt);

                sellByStation.TryGetValue(stationId, out var stationSells);
                response.AddInt(stationSells?.Count ?? 0);
                if (stationSells != null)
                    foreach (var order in stationSells)
                        response.AddLong(order.Id).AddString(order.ItemId).AddLong(order.Price)
                                .AddInt(order.Quantity).AddLong(order.ExpiresAt);
            }

            _networkManager.Server.Send(response, fromClientId);
        }

        private static List<T> GetOrCreate<T>(Dictionary<int, List<T>> map, int key)
        {
            if (!map.TryGetValue(key, out var list))
            {
                list = new List<T>();
                map[key] = list;
            }

            return list;
        }

        private string GetStarSystemName(int starSystemId, Dictionary<int, string> cache)
        {
            if (!cache.TryGetValue(starSystemId, out var name))
            {
                name = _starsSystemRepository.GetById(starSystemId)?.Name ?? string.Empty;
                cache[starSystemId] = name;
            }

            return name;
        }

        internal void HandleRequestBuyItem(ushort fromClientId, Message message)
        {
            var orderId = message.GetLong();
            var quantity = message.GetInt();
            var messageId = message.GetUInt();

            var characterId = _playerCharacterManager.GetCharacterIdForPlayer(fromClientId);

            if (_purchaseService.TryBuyFromSellOrder(characterId, orderId, quantity, out var error))
            {
                var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseBuyItem)
                    .AddUInt(messageId);

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

        internal void HandleMoveInput(ushort fromClientId, Message message)
        {
            var moveInput = message.GetVector2();
            _serverInputService.SetPlayerMoveInput(fromClientId, moveInput);
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
