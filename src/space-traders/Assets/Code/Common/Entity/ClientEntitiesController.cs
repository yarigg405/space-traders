using Assets.Code.Serialization.Data;
using Assets.Code.Serialization.Extensions;
using Code.Common.Extensions;


namespace Assets.Code.Common.Entity
{
    public sealed class ClientEntitiesController
    {
        private readonly GameContext _gameContext;

        public ClientEntitiesController(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public void CreateEntityFromSnapshot(EntitySnapshot snapshot)
        {
            _gameContext.CreateEntity()
                .FillEntityWith(snapshot);
        }

        public void DestroyEntity(uint entityId)
        {
            _gameContext.GetEntityWithId(entityId)
                .With(x => x.isDestructed = true);
        }
    }
}
