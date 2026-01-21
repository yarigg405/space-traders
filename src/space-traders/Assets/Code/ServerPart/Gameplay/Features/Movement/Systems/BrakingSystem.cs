using Assets.Code.Common.Time;
using Entitas;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class BrakingSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly ITimeService _time;
        private const float _brakingModifier = 7f;

        public BrakingSystem(GameContext game, ITimeService time)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.Braking,
                GameMatcher.Velocity,
                GameMatcher.VelocityAgility
            ));
            _time = time;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var newVelocity = Vector2.MoveTowards(entity.Velocity, Vector2.zero, entity.VelocityAgility
                    * _brakingModifier * _time.DeltaTime);
                entity.ReplaceVelocity(newVelocity);
            }
        }
    }
}
