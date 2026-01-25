using Assets.Code.Common;
using Assets.Code.Common.Extensions;
using Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ServerPart.Networking;
using Assets.Code.ServerPart.Worlds;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction
{
    public sealed class ServerInputService
    {
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly ServerWorldsController _worldsController;
        private readonly ClientSceneConnector _clientSceneConnector;

        public ServerInputService(PlayerDataProvider playerDataProvider,
            ServerWorldsController worldsController, ClientSceneConnector clientSceneConnector)
        {
            _playerDataProvider = playerDataProvider;
            _worldsController = worldsController;
            _clientSceneConnector = clientSceneConnector;
        }

        public void SetPlayerTargetRotation(ushort fromClientId, float targetRotation)
        {
            var input = CreateNewInputEntityForPlayer(fromClientId)
                .AddTargetRotation(targetRotation);
        }

        public void SetPlayerSpeedModifier(ushort fromClientId, float targetSpeedModifier)
        {
            var input = CreateNewInputEntityForPlayer(fromClientId)
                .AddCurrentSpeedModifier(targetSpeedModifier);
        }

        public void SetPlayerKeepDistance(ushort fromClientId, uint targetId, Vector2 minMaxDistance)
        {
            var input = CreateNewInputEntityForPlayer(fromClientId)
                 .AddMovementTargetId(targetId)
                 .AddKeepDistanceMinMax(minMaxDistance);
        }

        public void SetPlayerOrbitMoving(ushort fromClientId, uint targetId, float orbitRadius)
        {
            var input = CreateNewInputEntityForPlayer(fromClientId)
                .AddMovementTargetId(targetId)
                .AddOrbitingRadius(orbitRadius);
        }

        public void SetPlayerWarpTo(ushort fromClientId, double2 coordinates)
        {
            var input = CreateNewInputEntityForPlayer(fromClientId)
                .AddWarpFinishCoordinates(coordinates);
        }

        private InputEntity CreateNewInputEntityForPlayer(ushort playerNetworkId)
        {
            var sceneName = _playerDataProvider.GetSceneNameForPlayer(playerNetworkId);
            var world = _worldsController.GetOrCreateWorld(sceneName);
            var ctxs = world.Contexts;

            var entityId = _clientSceneConnector.GetEntityIdForPlayer(playerNetworkId);

            var input = CreateEntity.EmptyInput(ctxs)
               .With(x => x.isInput = true)
               .AddInputConsumerEntityId(entityId);

            return input;
        }
    }
}
