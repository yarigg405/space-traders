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

        public PlayerBuilder(PlayerFactory playerFactory,
            ServerWorldsController worldsController, PlayerLocationManager playerLocationManager, PlayerCharacterManager playerCharacterManager)
        {
            _worldsController = worldsController;
            _playerFactory = playerFactory;
            _playerLocationManager = playerLocationManager;
            _playerCharacterManager = playerCharacterManager;
        }

        public GameEntity CreatePlayer(ushort clientId)
        {
            var characterId = _playerCharacterManager.GetCharacterIdForPlayer(clientId);
            var sceneName = _playerLocationManager.GetSceneForCharacter(characterId);
            var spawnPoint = _playerLocationManager.GetCoordinatesForSpawn(characterId);

            var world = _worldsController.GetOrCreateWorld(sceneName);
            var ctxs = world.Contexts;

            var newPlayerEntity = _playerFactory.CreatePlayer(clientId, spawnPoint, ctxs);
            return newPlayerEntity;
        }
    }
}
