using Assets.Code.Common.StaticData.Repositories;
using Assets.Code.ServerPart.Gameplay.Features.Movement;
using Assets.Code.ServerPart.Gameplay.Features.Player.Factory;
using Assets.Code.ServerPart.Worlds;


namespace Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerBuilder
    {
        private readonly PlayerFactory _playerFactory;
        private readonly ServerWorldsController _worldsController;
        private readonly PlayerCharacterManager _playerCharacterManager;
        private readonly PlayerLocationManager _playerLocationManager;
        private readonly CharacterShipsRepository _characterShipsRepository;

        public PlayerBuilder(PlayerFactory playerFactory,
            ServerWorldsController worldsController, PlayerLocationManager playerLocationManager, PlayerCharacterManager playerCharacterManager,
            CharacterShipsRepository characterShipsRepository)
        {
            _worldsController = worldsController;
            _playerFactory = playerFactory;
            _playerLocationManager = playerLocationManager;
            _playerCharacterManager = playerCharacterManager;
            _characterShipsRepository = characterShipsRepository;
        }

        public GameEntity CreatePlayer(ushort clientId, bool spawnFromStation)
        {
            var characterId = _playerCharacterManager.GetCharacterIdForPlayer(clientId);
            var worldKey = _playerLocationManager.GetWorldKeyForCharacter(characterId);
            var spawnPoint = _playerLocationManager.GetCoordinatesForSpawn(characterId);
            var shipModelId = _characterShipsRepository.GetCurrentShip(characterId).ShipModelId;

            var world = _worldsController.GetOrCreateWorld(worldKey);
            var ctxs = world.Contexts;

            var newPlayerEntity = _playerFactory.CreatePlayer(clientId, spawnPoint, ctxs, shipModelId);
            if (spawnFromStation)
            {
                newPlayerEntity.StartUndocking();
            }

            return newPlayerEntity;
        }
    }
}
