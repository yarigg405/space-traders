using Assets.Code.Common;
using Assets.Code.ServerPart.Gameplay.Features.Player.Factory;
using Assets.Code.ServerPart.Worlds;
using Unity.Mathematics;



namespace Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerBuilder
    {
        private readonly PlayerFactory _playerFactory;
        private readonly ServerWorldsController _worldsController;
        private readonly PlayerDataProvider _playerDataProvider;

        public PlayerBuilder(PlayerFactory playerFactory,
            ServerWorldsController worldsController, PlayerDataProvider playerDataProvider)
        {
            _worldsController = worldsController;
            _playerFactory = playerFactory;
            _playerDataProvider = playerDataProvider;
        }

        public GameEntity CreatePlayer(ushort clientId)
        {
            var sceneName = _playerDataProvider.GetSceneNameForPlayer(clientId);
            var world = _worldsController.GetOrCreateWorld(sceneName);
            var ctxs = world.Contexts;
            var spawnPoint = double2.zero.GetRandomCoordinatesAroundPointZX(25f);

            var newPlayerEntity = _playerFactory.CreatePlayer(clientId, spawnPoint, ctxs);
            return newPlayerEntity;
        }
    }
}
