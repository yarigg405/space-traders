using Assets.Code.Common;
using Assets.Code.Common.Extensions;
using Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ServerPart.Networking;
using Assets.Code.ServerPart.Worlds;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction
{
    public sealed class ServerInputService
    {
        private readonly PlayerLocationManager _playerDataProvider;
        private readonly ServerWorldsController _worldsController;
        private readonly ClientSceneConnector _clientSceneConnector;
        private readonly ServerCommandBuffer _commandBuffer;

        public ServerInputService(PlayerLocationManager playerDataProvider,
            ServerWorldsController worldsController, ClientSceneConnector clientSceneConnector, ServerCommandBuffer commandBuffer)
        {
            _playerDataProvider = playerDataProvider;
            _worldsController = worldsController;
            _clientSceneConnector = clientSceneConnector;
            _commandBuffer = commandBuffer;
        }
        

        internal void SetPlayerMoveInput(ushort fromClientId, uint tick, Vector2 moveInput)
        {
            var entityId = _clientSceneConnector.GetEntityIdForPlayer(fromClientId);
            if (entityId == 0) return;
            _commandBuffer.Store(entityId, tick, moveInput);
        }

        internal void SetPlayerKeepDistance(ushort fromClientId, uint targetId, Vector2 minMaxDistance)
        {
            CreateNewInputEntityForPlayer(fromClientId)?
                .AddMovementTargetId(targetId)
                .AddKeepDistanceMinMax(minMaxDistance);
        }

        internal void SetPlayerOrbitMoving(ushort fromClientId, uint targetId, float orbitRadius)
        {
            CreateNewInputEntityForPlayer(fromClientId)?
                .AddMovementTargetId(targetId)
                .AddOrbitingRadius(orbitRadius);
        }

        internal void SetPlayerWarpTo(ushort fromClientId, double2 coordinates)
        {
            CreateNewInputEntityForPlayer(fromClientId)?
                .AddWarpFinishCoordinates(coordinates);
        }

        private InputEntity CreateNewInputEntityForPlayer(ushort playerNetworkId)
        {
            var worldKey = _playerDataProvider.GetWorldKeyForCharacter(playerNetworkId);
            var world = _worldsController.GetOrCreateWorld(worldKey);
            var ctxs = world.Contexts;

            var entityId = _clientSceneConnector.GetEntityIdForPlayer(playerNetworkId);

            var player = ctxs.game.GetEntityWithId(entityId);
            if (player == null && (player.isWarpPreparation || player.isWarping))
                return null;

            var input = CreateEntity.EmptyInput(ctxs)
               .With(x => x.isInput = true)
               .AddInputConsumerEntityId(entityId);

            return input;
        }
    }
}
