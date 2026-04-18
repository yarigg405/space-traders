using Assets.Code.ClientPart.Networking;
using Assets.Code.Common.Serialization;
using Assets.Code.Common.Serialization.Data;
using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.Networking;
using Assets.Code.ServerPart.Gameplay.Features.InputInteraction;
using Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure;
using Riptide;
using Unity.Mathematics;
using VContainer;


namespace Assets.Code.ServerPart.Networking
{
    public static class ServerMessenger
    {
        private static NetworkManager _networkManager;
        private static ClientSceneConnector _clientSceneConnector;
        private static PlayerDataProvider _playerDataProvider;
        private static ServerInputService _serverInputService;
        private static CharactersRepository _charactersRepository;
        private static PlayersRepository _playersRepository;

        public static void SetupDependencies(IObjectResolver resolver)
        {
            _networkManager = resolver.Resolve<NetworkManager>();
            _clientSceneConnector = resolver.Resolve<ClientSceneConnector>();
            _playerDataProvider = resolver.Resolve<PlayerDataProvider>();
            _serverInputService = resolver.Resolve<ServerInputService>();

            _playersRepository = resolver.Resolve<PlayersRepository>();
            _charactersRepository = resolver.Resolve<CharactersRepository>();
        }

        public static void SendConnectionDataToPlayer(ushort clientId, string sceneName)
        {
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.ResponseEnterTheGame)
                .AddString(sceneName);

            _networkManager.Server.Send(message, clientId);
        }

        public static void SendEntityToClient(ushort clientId, EntitySnapshot snapshot)
        {
            var json = JsonSerializator.ToJson(snapshot);
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.CreateEntity)
                .AddString(json);

            _networkManager.Server.Send(message, clientId);
        }

        public static void DestroyEntityOnClient(ushort clientId, uint entityId)
        {
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.DestroyEntity)
                .AddUInt(entityId);

            _networkManager.Server.Send(message, clientId);
        }

        #region Update of components
        public static void SynchronizeGlobalPosition(ushort client, uint entityId, double2 globalPosition)
        {
            var message = Message.Create(MessageSendMode.Unreliable, ServerToClientMessageType.SynchronizeGlobalPosition)
                .AddUInt(entityId)
                .AddDouble(globalPosition.x)
                .AddDouble(globalPosition.y)
                ;

            _networkManager.Server.Send(message, client);
        }

        public static void SynchronizeRotation(ushort client, uint entityId, float currentRotation)
        {
            var message = Message.Create(MessageSendMode.Unreliable, ServerToClientMessageType.SynchronizeRotation)
                .AddUInt(entityId)
                .AddFloat(currentRotation)
                ;

            _networkManager.Server.Send(message, client);
        }

        public static void UpdateComponentsForEntity(ushort client, uint entityId, string snapshotJson)
        {
            var message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.UpdateComponentsForEntity)
                .AddUInt(entityId)
                .AddString(snapshotJson);

            _networkManager.Server.Send(message, client);
        }

        #endregion



        #region MessageHandlers


        //[MessageHandler((ushort)ClientToServerMessageType.RequestConnectToGame)]
        //private static void HandleConnectToGameScene(ushort fromClientId, Message message)
        //{
        //    _clientSceneConnector.ConnectPlayer(fromClientId);
        //}
        [MessageHandler((ushort)ClientToServerMessageType.RequestGetCharacters)]
        private static void HandleRequestGetCharacters(ushort fromClientId, Message message)
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
                    response.AddString(character.Name);
                }

                _networkManager.Server.Send(response, fromClientId);
            }

            else
            {
                var response = Message.Create(MessageSendMode.Reliable, ServerToClientMessageType.RequestFailed)
                    .AddUInt(messageId)
                    .AddString("Wrong password");

                _networkManager.Server.Send(response, fromClientId);
            }
        }

        [MessageHandler((ushort)ClientToServerMessageType.RequestForSceneEntities)]
        private static void HandleEntitiesLoading(ushort fromClientId, Message message)
        {
            _clientSceneConnector.FillWorldForClient(fromClientId);
        }

        [MessageHandler((ushort)ClientToServerMessageType.RequestForChangeScene)]
        private static void HandleChangeScene(ushort fromClientId, Message message)
        {
            var sceneName = message.GetString();
            var scene = _playerDataProvider.GetSceneNameForPlayer(fromClientId);
            if (scene.Equals(sceneName)) return;
            _playerDataProvider.SetPlayerScene(fromClientId, sceneName);

            _clientSceneConnector.ConnectPlayer(fromClientId);
        }

        [MessageHandler((ushort)ClientToServerMessageType.SendTargetRotation)]
        private static void HandleClientTargetRotation(ushort fromClientId, Message message)
        {
            var targetRotation = message.GetFloat();
            _serverInputService.SetPlayerTargetRotation(fromClientId, targetRotation);
        }

        [MessageHandler((ushort)ClientToServerMessageType.SendSpeedModifier)]
        private static void HandleClientSpeedModifier(ushort fromClientId, Message message)
        {
            var speedModifier = message.GetFloat();
            _serverInputService.SetPlayerSpeedModifier(fromClientId, speedModifier);
        }

        [MessageHandler((ushort)ClientToServerMessageType.SendKeepDistance)]
        private static void HandleKeepDistance(ushort fromClientId, Message message)
        {
            var targetId = message.GetUInt();
            var minMaxDistance = message.GetVector2();

            _serverInputService.SetPlayerKeepDistance(fromClientId, targetId, minMaxDistance);
        }

        [MessageHandler((ushort)ClientToServerMessageType.SendSetOrbit)]
        private static void HandleSetOrbig(ushort fromClientId, Message message)
        {
            var targetId = message.GetUInt();
            var orbitRadius = message.GetFloat();

            _serverInputService.SetPlayerOrbitMoving(fromClientId, targetId, orbitRadius);
        }

        [MessageHandler((ushort)ClientToServerMessageType.SendSetWarpTo)]
        private static void HandleSetWarpTo(ushort fromClientId, Message message)
        {
            var x = message.GetDouble();
            var y = message.GetDouble();
            var coordinates = new double2(x, y);

            _serverInputService.SetPlayerWarpTo(fromClientId, coordinates);
        }
        #endregion
    }
}
