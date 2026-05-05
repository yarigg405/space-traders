using Assets.Code.Common.DataBase.ORM;
using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.Common.StaticData.Staff;
using Assets.Code.Infrastructure.Loading;
using Assets.Code.Networking;
using Assets.Code.ServerPart.Gameplay.Features.InputInteraction;
using Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure;
using Riptide;
using System;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Networking
{
    internal sealed class ServerMessengerRouter
    {
        private readonly NetworkManager _networkManager;
        private readonly ServerInputService _serverInputService;
        private readonly CharactersCreatingService _characterCreator;
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly ClientSceneConnector _clientSceneConnector;

        private readonly PlayersRepository _playersRepository;
        private readonly CharactersRepository _charactersRepository;
        private readonly CharacterLocationsRepository _characterLocationsRepository;
        private readonly StarSystemRepository _starsSystemRepository;
        private readonly SpaceStationsRepository _spaceStationsRepository;


        public ServerMessengerRouter(NetworkManager networkManager,
            PlayersRepository playersRepository,
            CharactersRepository charactersRepository,
            ServerInputService serverInputService,
            CharactersCreatingService characterCreator,
            CharacterLocationsRepository characterLocationsRepository,
            StarSystemRepository starsSystemRepository,
            ClientSceneConnector clientSceneConnector,
            PlayerDataProvider playerDataProvider,
            SpaceStationsRepository spaceStationsRepository)
        {
            _networkManager = networkManager;
            _playersRepository = playersRepository;
            _charactersRepository = charactersRepository;
            _serverInputService = serverInputService;
            _characterCreator = characterCreator;
            _characterLocationsRepository = characterLocationsRepository;
            _starsSystemRepository = starsSystemRepository;
            _clientSceneConnector = clientSceneConnector;
            _playerDataProvider = playerDataProvider;
            _spaceStationsRepository = spaceStationsRepository;
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

            var location = _characterLocationsRepository.GetLocationForCharacter(characterId);

            string sceneName = string.Empty;
            if (location.LocationType == LocationType.Space)
            {
                sceneName = _starsSystemRepository.Get(location.CurrentLocationId).Name;
            }

            else //if (location.LocationType == LocationType.Station)
            {
                sceneName = SceneNames.StationScene;
            }

            _playerDataProvider.SetPlayerScene(fromClientId, sceneName);
            _clientSceneConnector.ConnectPlayer(fromClientId);
            var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseEnterTheGame)
                .AddUInt(messageId)
                .AddString(sceneName);

            _networkManager.Server.Send(response, fromClientId);
        }

        internal void HandleRequestForStationSceneData(ushort fromClientId, Message message)
        {
            var characterId = message.GetInt();
            var messageId = message.GetUInt();
            var location = _characterLocationsRepository.GetLocationForCharacter(characterId);
            var station = _spaceStationsRepository.GetById(location.CurrentLocationId);

            var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseLoadStationData)
                .AddUInt(messageId)
                .AddInt(station.Id)
                .AddString(station.Name)
                ;

            _networkManager.Server.Send(response, fromClientId);
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
