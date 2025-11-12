using Assets.Code.ClientPart.CameraSystem;
using Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Networking;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Systems
{
    internal sealed class BindPlayerServicesSystem : ReactiveSystem<GameEntity>
    {
        private readonly ICameraService _cameraService;
        private readonly IPlayerProviderSetupper _playerProvider;
        private readonly NetworkManager _networkManager;

        public BindPlayerServicesSystem(GameContext game, ICameraService cameraService,
            IPlayerProviderSetupper playerProvider, NetworkManager networkManager)
            : base(game)
        {
            _cameraService = cameraService;
            _playerProvider = playerProvider;
            _networkManager = networkManager;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(
                GameMatcher.Player,
                GameMatcher.PlayerNetworkId,
                GameMatcher.Transform)
                .Added());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isPlayer &&
                entity.hasPlayerNetworkId &&
                entity.hasTransform;
        }

        protected override void Execute(List<GameEntity> players)
        {
            foreach (var player in players)
            {
                if (player.PlayerNetworkId == _networkManager.Client.Id)
                {
                    _playerProvider.SetPlayer(player);
                    _cameraService.SetTarget(player.Transform);
                }
            }
        }
    }
}