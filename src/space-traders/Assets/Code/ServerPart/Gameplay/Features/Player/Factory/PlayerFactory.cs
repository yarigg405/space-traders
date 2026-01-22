using Assets.Code.Common;
using Assets.Code.Common.Extensions;
using Assets.Code.Infrastructure.Identifiers;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Player.Factory
{
    public sealed class PlayerFactory
    {
        private readonly IIdentifierService _identifier;

        public PlayerFactory(IIdentifierService identifier)
        {
            _identifier = identifier;
        }

        public GameEntity CreatePlayer(ushort playerNetworkId, double2 at, Contexts contexts)
        {
            var player = CreateEntity.Empty(contexts)
                .AddId(_identifier.Next())
                .AddLocalPosition(Vector3.zero)
                .AddPlayerNetworkId(playerNetworkId)
                .AddViewPath("Prefabs/PlayerShip")
                .With(x => x.isPlayer = true)

                .AddGlobalPosition(at)
                .With(x => x.isMoving = true)
                .AddVelocity(Vector2.zero)
                .AddCurrentRotationY(0)
                .AddTargetRotation(0)

                .AddCurrentSpeedModifier(0)
                .AddCurrentMoveSpeed(0)

                .AddVelocityAgility(2.5f)
                .AddRotationSpeed(250f)
                .AddMaxMoveSpeed(15f)
                .AddMovingAcceleration(3f)
                ;

            return player;
        }
    }
}
