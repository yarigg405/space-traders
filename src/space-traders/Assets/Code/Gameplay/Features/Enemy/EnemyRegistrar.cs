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
                .AddMoveSpeed(4f)

                .AddCurrentRotationY(0)
                .AddTargetRotation(0)
                .AddRotationSpeed(30f)

                .AddChaseTarget(_chaseTarget.Entity);
                ;
        }

        public override void UnRegisterComponents()
        {
           
        }
    }
}
