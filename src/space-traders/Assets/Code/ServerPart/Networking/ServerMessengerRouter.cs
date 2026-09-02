using Assets.Code.Common.DataBase.ORM;
using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.Common.StaticData.Staff;
using Assets.Code.Common.Time;
using Assets.Code.Networking;
using Assets.Code.ServerPart.Gameplay.Features.InputInteraction;
using Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ServerPart.Gameplay.Features.Player.Services;
using Assets.Code.ServerPart.Gameplay.Features.Trading;
using Riptide;
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
        private readonly ItemStacksRepository _itemStacksRepository;
        private readonly PurchaseService _purchaseService;
        private readonly SellService _sellService;
        private readonly TickCounter _tickCounter;


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
            ItemStacksRepository itemStacksRepository,
            PurchaseService purchaseService,
            SellService sellService,
            PlayerLocationManager playerLocationManager,
            PlayerCharacterManager playerCharacterManager,
            ServerDockingService dockingService,
            TickCounter tickCounter)
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
            _itemStacksRepository = itemStacksRepository;
            _purchaseService = purchaseService;
            _sellService = sellService;
            _playerLocationManager = playerLocationManager;
            _playerCharacterManager = playerCharacterManager;
            _dockingService = dockingService;
            _tickCounter = tickCounter;
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

        internal void HandleRequestSellItem(ushort fromClientId, Message message)
        {
            var orderId = message.GetLong();
            var quantity = message.GetInt();
            var messageId = message.GetUInt();

            var characterId = _playerCharacterManager.GetCharacterIdForPlayer(fromClientId);

            if (_sellService.TrySellToBuyOrder(characterId, orderId, quantity, out var error))
            {
                var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseSellItem)
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

        internal void HandleRequestPlayerInventory(ushort fromClientId, Message message)
        {
            var messageId = message.GetUInt();
            var characterId = _playerCharacterManager.GetCharacterIdForPlayer(fromClientId);

            var stacks = _itemStacksRepository.GetStationStacksByOwner(characterId);

            var byStation = new Dictionary<int, Dictionary<string, int>>();
            foreach (var stack in stacks)
            {
                if (!byStation.TryGetValue(stack.ContainerId, out var items))
                {
                    items = new Dictionary<string, int>();
                    byStation[stack.ContainerId] = items;
                }

                items.TryGetValue(stack.ItemId, out var amount);
                items[stack.ItemId] = amount + stack.Amount;
            }

            var systemNames = new Dictionary<int, string>();

            var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponsePlayerInventory)
                .AddUInt(messageId)
                .AddInt(byStation.Count);

            foreach (var stationPair in byStation)
            {
                var station = _spaceStationsRepository.GetById(stationPair.Key);

                response.AddInt(stationPair.Key)
                        .AddString(station.Name)
                        .AddString(GetStarSystemName(station.StarSystemId, systemNames))
                        .AddInt(stationPair.Value.Count);

                foreach (var itemPair in stationPair.Value)
                    response.AddString(itemPair.Key).AddInt(itemPair.Value);
            }

            _networkManager.Server.Send(response, fromClientId);
        }

        internal void HandleRequestBestBuyOrder(ushort fromClientId, Message message)
        {
            var itemId = message.GetString();
            var stationId = message.GetInt();
            var messageId = message.GetUInt();

            BuyOrderORM best = null;
            foreach (var order in _buyOrdersRepository.GetByStation(stationId))
            {
                if (order.ItemId != itemId || order.Quantity <= 0)
                    continue;

                if (best == null || order.Price > best.Price)
                    best = order;
            }

            var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseBestBuyOrder)
                .AddUInt(messageId)
                .AddBool(best != null);

            if (best != null)
                response.AddLong(best.Id).AddLong(best.Price).AddInt(best.Quantity);

            _networkManager.Server.Send(response, fromClientId);
        }

        internal void HandleEntitiesLoading(ushort fromClientId, Message message)
        {
            _clientSceneConnector.FillWorldForClient(fromClientId);
        }
       

        internal void HandleMoveInput(ushort fromClientId, Message message)
        {
            var tick = message.GetUInt();
            var moveInput = message.GetVector2();
            _serverInputService.SetPlayerMoveInput(fromClientId, tick, moveInput);
        }


        internal void HandleKeepDistance(ushort fromClientId, Message message)
        {
            var targetId = message.GetUInt();
            var minMaxDistance = message.GetVector2();

            _serverInputService.SetPlayerKeepDistance(fromClientId, targetId, minMaxDistance);
        }

        internal void HandleSetOrbit(ushort fromClientId, Message message)
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

        internal void HandlePing(ushort fromClientId, Message message)
        {
            var pingId = message.GetUInt();

            var response = Message.Create(MessageSendMode.Unreliable,
                ServerToClientMessageType.Pong)
                .AddUInt(pingId)
                .AddUInt(_tickCounter.CurrentTick);

            _networkManager.Server.Send(response, fromClientId);
        }
    }
}
