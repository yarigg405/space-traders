using Assets.Code.View;
using Code.Common.Extensions;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.Player.Registrars
{
    internal sealed class HeroRegistrar : EntityComponentRegistrar
    {
        public override void RegisterComponents()
        {
            Entity
                .AddGlobalPosition(new Common.Vector2Double(transform.position.x, transform.position.z))
                .AddLocalPosition(Vector3.zero)

                .AddVelocity(Vector2.zero)
                .AddVelocityAgility(50f)

                .AddCurrentRotationY(0)
                .AddTargetRotation(0)
                .AddRotationSpeed(50f)

                .AddRadius(10f)
                .AddMaxHp(50f)
                .AddCurrentHp(50f)

                .With(x => x.isPlayer = true);
        }

        public override void UnRegisterComponents()
        {

        }
    }
}
