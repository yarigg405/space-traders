using Assets.Code.Common.DataContainers;
using Assets.Code.ServerPart.Worlds.GameSynchronization;
using Entitas;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class CheckShipCanBeDockedSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _gameContext;
        private readonly EntitiesSynchronizator _synchronizator;

        public CheckShipCanBeDockedSystem(GameContext game, EntitiesSynchronizator synchronizator)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.StationDockingBay,
                GameMatcher.TriggerEnterEventHandler,
                GameMatcher.TriggerExitEventHandler,
                GameMatcher.ShipsInDockZone
                ));
            _gameContext = game;
            _synchronizator = synchronizator;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                foreach (var entered in entity.TriggerEnterEventHandler)
                {
                    var triggered = _gameContext.GetEntityWithId(entered);
                    if (triggered.isPlayer)
                        entity.ShipsInDockZone.Add(triggered);
                }

                foreach (var exited in entity.TriggerExitEventHandler)
                {
                    var triggered = _gameContext.GetEntityWithId(exited);
                    if (triggered.isPlayer)
                    {
                        entity.ShipsInDockZone.Remove(triggered);
                        SetCanNotBeDocked(triggered);
                    }
                }

                foreach (var ship in entity.ShipsInDockZone)
                {
                    if (ship.CurrentMoveSpeed < 0.1f)
                    {
                        SetCanBeDocked(ship, entity.ParentEntity, entity.Id);
                    }

                    else
                    {
                        SetCanNotBeDocked(ship);
                    }
                }
            }
        }

        private void SetCanBeDocked(GameEntity entity, uint stationId, uint dockingBayId)
        {
            if (entity.hasShipCanBeDocked) return;

            entity.AddShipCanBeDocked(new DockingDataContainer
            {
                Dbid = dockingBayId,
                StId = stationId,
            });
            _synchronizator.UpdateComponentsForEntity(entity, GameComponentsLookup.ShipCanBeDocked);
        }

        private void SetCanNotBeDocked(GameEntity entity)
        {
            if (!entity.hasShipCanBeDocked) return;

            entity.RemoveShipCanBeDocked();
            _synchronizator.UpdateComponentsForEntity(entity, GameComponentsLookup.ShipCanBeDocked);
        }
    }
}
