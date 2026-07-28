using Assets.Code.ClientPart.CameraSystem;
using Assets.Code.Common;
using Entitas;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.SkyboxObjects.Systems
{
    internal sealed class UpdateSkyboxLocalPositionSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _players;
        private readonly IGroup<GameEntity> _entities;
        private readonly SkyboxSpaceState _skyboxSpace;

        public UpdateSkyboxLocalPositionSystem(
            GameContext game,
            SkyboxSpaceState backgroundSpace)
        {
            _players = game.GetGroup(GameMatcher.ClientPlayer);

            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.SkyboxCoordinates,
                GameMatcher.QuadrantIndex
            ));

            _skyboxSpace = backgroundSpace;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var planetGlobal = entity.SkyboxCoordinates;
                var anchor = _skyboxSpace.SkyboxAnchor;

                var dx = planetGlobal.x - anchor.x;
                var dy = planetGlobal.y - anchor.y;

                float scale = GameConstants.SKYBOX_OBJECTS_POSITION_MODIFIER;

                var newLocal = new Vector3(
                    (float)(dx * scale),
                    0f,
                    (float)(dy * scale)
                );

                entity.ReplaceLocalPosition(newLocal);
            }
        }
    }
}
