using Entitas;
using Yrr.Utils;


namespace Assets.Code.ServerPart.Gameplay.Features.Movement.Systems
{
    internal sealed class KeepDistanceSystem : IExecuteSystem
    {
        private readonly IGroup<GameEntity> _entities;
        private readonly GameContext _game;

        public KeepDistanceSystem(GameContext game)
        {
            _entities = game.GetGroup(GameMatcher.AllOf(
                GameMatcher.MovementTargetId,
                GameMatcher.KeepDistanceMinMax
                ));
            _game =     game;
        }

        void IExecuteSystem.Execute()
        {
            foreach (var entity in _entities)
            {
                var target = _game.GetEntityWithId(entity.MovementTargetId).Transform;
                var minMaxDistance = entity.KeepDistanceMinMax;

                var distance = (entity.Transform.position - target.position).magnitude;

                if (distance < minMaxDistance.x)
                {
                    var angle = AnglesUtil.GetAngleDirectionY(target.position, entity.Transform.position);
                    entity.ReplaceTargetRotation(angle);
                    entity.ReplaceBraking(false);

                    if (entity.CurrentSpeedModifier == 0)
                        entity.ReplaceCurrentSpeedModifier(1f);
                }

                else if (distance > minMaxDistance.y)
                {
                    var angle = AnglesUtil.GetAngleDirectionY(entity.Transform.position, target.position);
                    entity.ReplaceTargetRotation(angle);
                    entity.ReplaceBraking(false);

                    if (entity.CurrentSpeedModifier == 0)
                        entity.ReplaceCurrentSpeedModifier(1f);
                }

                else
                {
                    entity.ReplaceCurrentSpeedModifier(0);
                    entity.ReplaceBraking(true);
                }
            }
        }
    }
}
