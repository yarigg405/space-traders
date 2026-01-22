using Assets.Code.Common;
using Assets.Code.Common.Components;
using Assets.Code.Common.Extensions;
using Assets.Code.ServerPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.ServerPart.Worlds;
using System;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.InputInteraction
{
    public sealed class ServerInputService
    {
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly ServerWorldsController _worldsController;

        public ServerInputService(PlayerDataProvider playerDataProvider, ServerWorldsController worldsController)
        {
            _playerDataProvider = playerDataProvider;
            _worldsController = worldsController;
        }

        public void SetPlayerDoubleClick(ushort playerNetworkId, Vector3 clickPosition)
        {
            var sceneName = _playerDataProvider.GetSceneNameForPlayer(playerNetworkId);
            var world = _worldsController.GetOrCreateWorld(sceneName);
            var ctxs = world.Contexts;

            var input = CreateEntity.EmptyInput(ctxs)
                .With(x => x.isInput = true)
                .AddClickedPosition(clickPosition)
                .AddInputPlayerTarget(playerNetworkId)
                ;
        }

        public void SetPlayerTargetRotation(ushort fromClientId, float targetRotation)
        {
            var sceneName = _playerDataProvider.GetSceneNameForPlayer(fromClientId);
            var world = _worldsController.GetOrCreateWorld(sceneName);
            var ctxs = world.Contexts;

            var input = CreateEntity.EmptyInput(ctxs)
                .With(x => x.isInput = true)
                .AddTargetRotation(targetRotation)
                .AddInputPlayerTarget(fromClientId)
                ;
        }
    }
}
