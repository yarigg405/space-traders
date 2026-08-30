using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;
using System.Collections.Generic;


namespace Assets.Code.ServerPart.Gameplay.Features.Synchronization.Systems
{
    internal sealed class DockingProcessSynchronizationSystem : ReactiveSystem<GameEntity>
    {
        private readonly EntitiesSynchronizator _synchronizator;

        public DockingProcessSynchronizationSystem(GameContext game, EntitiesSynchronizator synchronizator) : base(game)
        {
            _synchronizator = synchronizator;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.AllOf(
                GameMatcher.DockingInProcess)
                 .Added());
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.isDockingInProcess;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var entity in entities)
            {
                _synchronizator.UpdateComponentsForEntity(entity,
                    GameComponentsLookup.DockingInProcess,
                    GameComponentsLookup.ShipCanBeDocked);
            }
        }
    }
}
