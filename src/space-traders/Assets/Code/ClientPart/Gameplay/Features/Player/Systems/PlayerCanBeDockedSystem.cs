using Assets.Code.UI.Infrastructure;
using Assets.Code.UI.Screens;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Systems
{
    internal sealed class PlayerCanBeDockedSystem : ReactiveSystem<GameEntity>
    {
        private readonly IUIManager _uiManager;

        public PlayerCanBeDockedSystem(GameContext game, IUIManager uiManager) : base(game)
        {
            _uiManager = uiManager;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(
                GameMatcher.ClientPlayer,
                GameMatcher.ShipCanBeDocked)
                .Added());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isClientPlayer;
        }


        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                _uiManager.OpenScreen<RequestDockPopup>(entity.ShipCanBeDocked);
            }
        }
    }
}
