using Assets.Code.Common.DataBase.ORM;
using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.Networking;
using Assets.Code.ServerPart.Gameplay.Features.InputInteraction;
using Riptide;
using Unity.Mathematics;


namespace Assets.Code.ServerPart.Networking
{
    internal sealed class ServerMessengerRouter
    {
        private readonly NetworkManager _networkManager;
        private readonly PlayersRepository _playersRepository;
        private readonly CharactersRepository _charactersRepository;
        private readonly ServerInputService _serverInputService;


        public ServerMessengerRouter(NetworkManager networkManager,
            PlayersRepository playersRepository,
            CharactersRepository charactersRepository,
            ServerInputService serverInputService)
        {
            _networkManager = networkManager;
            _playersRepository = playersRepository;
            _charactersRepository = charactersRepository;
            _serverInputService = serverInputService;
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
                    .AddString("error-password");

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

            if (_charactersRepository.TryCreateNewCharacter(player.Id, character, out string error))
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
