 using Assets.Code.Common.Entity;
using Code.Common.Extensions;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.Player.Registrars
{
    internal sealed class HeroRegistrar : MonoBehaviour
    {
        private GameEntity _entity;

        private void Awake()
        {
            _entity = CreateEntity
                .Empty()
                .AddGlobalPosition(new Common.Vector2Double(transform.position.x, transform.position.z))
                .AddLocalPosition(Vector3.zero)
                .AddTransform(transform)

                .AddVelocity(Vector2.zero)
                .AddVelocityAgility(50f)

                .AddCurrentRotationY(0)
                .AddTargetRotation(0)
                .AddRotationSpeed(50f)

                .With(x => x.isPlayer = true)
            ;
        }
    }
}
