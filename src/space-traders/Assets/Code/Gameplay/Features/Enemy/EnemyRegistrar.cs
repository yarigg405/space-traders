using Assets.Code.View;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.Enemy
{
    internal sealed class EnemyRegistrar : EntityComponentRegistrar
    {
        [SerializeField] private EntityBehaviour _chaseTarget;

        public override void RegisterComponents()
        {
            Entity
                .AddGlobalPosition(new Common.Vector2Double(transform.position.x, transform.position.z))
                .AddLocalPosition(transform.position)

                .AddVelocity(Vector2.zero)
                .AddVelocityAgility(30f)
                .AddMaxMoveSpeed(20)

                .AddCurrentRotationY(0)
                .AddTargetRotation(0)
                .AddRotationSpeed(30f)

                .AddMaxHp(50f)
                .AddCurrentHp(50f)
                .AddDamage(30f)

                .AddRadius(10f)
                .AddCollectTargetsInterval(0.5f)
                .AddCollectTargetsTimer(0f)

                .AddChaseTarget(_chaseTarget.Entity);
            ;
        }

        public override void UnRegisterComponents()
        {

        }
    }
}
