using Assets.Code.Common.Entity;
using Assets.Code.Gameplay.Common;
using Assets.Code.Gameplay.Features.Player.Infrastructure;
using Assets.Code.Infrastructure.Identifiers;
using Code.Common.Extensions;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.Player.Factory
{
    public sealed class PlayerFactory
    {
        private readonly IIdentifierService _identifier;
        private readonly PlayerProvider _playerProvider;

        public PlayerFactory(IIdentifierService identifier, PlayerProvider playerProvider)
        {
            _identifier = identifier;
            _playerProvider = playerProvider;
        }

        public GameEntity CreatePlayer(Vector2Double at)
        {
            var player = CreateEntity.Empty()
                .AddId(_identifier.Next())
                .AddGlobalPosition(at)
                .AddLocalPosition(Vector3.zero)

                .AddVelocity(Vector2.zero)
                .AddVelocityAgility(50f)

                .AddCurrentRotationY(0)
                .AddTargetRotation(0)
                .AddRotationSpeed(50f)

                .AddRadius(10f)
                .AddMaxHp(50f)
                .AddCurrentHp(50f)

                .AddMaxMoveSpeed(15f)
                .AddCurrentSpeedModifier(0f)
                .AddCurrentMoveSpeed(0f)
                .AddMovingAcceleration(3f)

                .AddViewPath("Prefabs/PlayerShip")

                .With(x => x.isPlayer = true);

            _playerProvider.SetPlayer(player);

            return player;
        }
    }
}
