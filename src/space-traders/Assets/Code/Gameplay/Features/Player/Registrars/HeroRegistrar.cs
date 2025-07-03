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
                .AddDirectionalMovement(Vector2.zero)
                .AddTransform(transform)
                .With(x => x.isPlayer = true)
            ;

        }
    }
}
