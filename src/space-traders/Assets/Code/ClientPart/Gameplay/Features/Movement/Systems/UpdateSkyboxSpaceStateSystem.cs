using Assets.Code.ClientPart.CameraSystem;
using Assets.Code.Common;
using Entitas;
using Unity.Mathematics;


namespace Assets.Code.ClientPart.Gameplay.Features.Movement.Systems
{
    internal sealed class UpdateSkyboxSpaceStateSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly SkyboxSpaceState _skyboxSpaceState;


        public UpdateSkyboxSpaceStateSystem(GameContext game, SkyboxSpaceState skyboxSpaceState)
        {
            _players = game.GetGroup(GameMatcher.ClientPlayer);
            _skyboxSpaceState = skyboxSpaceState;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var player in _players)
            {
                var globalPosition = player.GlobalPosition;
                var playerQuadrant = player.QuadrantIndex;

                _skyboxSpaceState.SkyboxAnchor = new double2(
                    playerQuadrant.x * GameConstants.GAME_SCENE_QUADRANT_SIZE,
                    playerQuadrant.y * GameConstants.GAME_SCENE_QUADRANT_SIZE
                    );
            }
        }
    }
}
