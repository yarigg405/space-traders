using Entitas;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Physics.Systems
{
    internal sealed class ShipDockingSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _gameContext;

        public ShipDockingSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.StationDockingBay,
                GameMatcher.TriggerEnterEventHandler,
                GameMatcher.TriggerExitEventHandler
                ));
            _gameContext = game;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                foreach (var entered in entity.TriggerEnterEventHandler)
                {
                    var triggered = _gameContext.GetEntityWithId(entered);
                    if (triggered.isPlayer)
                        Debug.Log($"Player {triggered.PlayerNetworkId} entered to {entity.ParentEntity}-{entity.Id}");
                }

                foreach (var exited in entity.TriggerExitEventHandler)
                {
                    var triggered = _gameContext.GetEntityWithId(exited);
                    if (triggered.isPlayer)
                        Debug.Log($"Player {triggered.PlayerNetworkId} exited from {entity.ParentEntity}-{entity.Id}");
                }
            }
        }
    }
}
