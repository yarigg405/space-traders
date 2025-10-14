using Assets.Code.Gameplay.Common.CameraSystem;
using Assets.Code.Gameplay.Features.Player.Infrastructure;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.Gameplay.Features.Player.Systems
{
    internal sealed class BindPlayerServicesSystem : ReactiveSystem<GameEntity>
    {
        private readonly CameraService _cameraService;
        private readonly IPlayerProviderSetupper _playerProvider;


        public BindPlayerServicesSystem(GameContext game, CameraService cameraService, IPlayerProviderSetupper playerProvider)
            : base(game)
        {
            _cameraService = cameraService;
            _playerProvider = playerProvider;
        }


        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(
                GameMatcher.Player,
                GameMatcher.Transform
                ).Added());
        }
        protected override bool Filter(GameEntity entity)
        {
            return entity.isPlayer && entity.hasTransform;
        }

        protected override void Execute(List<GameEntity> players)
        {
            foreach (var player in players)
            {
                _playerProvider.SetPlayer(player);
                _cameraService.SetTarget(player.Transform);
            }
        }
    }
}
