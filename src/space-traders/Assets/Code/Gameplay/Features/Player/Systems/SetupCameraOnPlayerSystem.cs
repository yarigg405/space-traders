using Assets.Code.Gameplay.CameraSystem;
using Entitas;


namespace Assets.Code.Gameplay.Features.Player.Systems
{
    internal sealed class SetupCameraOnPlayerSystem : IInitializeSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly CameraService _cameraService;

        internal SetupCameraOnPlayerSystem(GameContext game, CameraService cameraService)
        {
            _players = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Player,
                GameMatcher.Transform
            ));
            _cameraService = cameraService;
        }

        void IInitializeSystem.Initialize()
        {
            foreach (var player in _players)
            {
                _cameraService.SetTarget(player.Transform);
            }
        }
    }
}