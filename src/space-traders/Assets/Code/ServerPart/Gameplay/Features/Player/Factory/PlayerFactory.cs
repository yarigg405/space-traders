using Assets.Code.Common;
using Assets.Code.Common.Extensions;
using Assets.Code.Common.Inventory.Components;
using Assets.Code.Common.StaticData;
using Assets.Code.Infrastructure.Identifiers;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ServerPart.Gameplay.Features.Player.Factory
{
    public sealed class PlayerFactory
    {
        private readonly IIdentifierService _identifier;
        private readonly IItemsCatalog _itemsCatalog;

        public PlayerFactory(IIdentifierService identifier, IItemsCatalog itemsCatalog)
        {
            _identifier = identifier;
            _itemsCatalog = itemsCatalog;
        }

        public GameEntity CreatePlayer(ushort playerNetworkId, double2 at, Contexts contexts, string shipModelId)
        {
            var shipItem = _itemsCatalog.GetItem(shipModelId);
            var shipComponent = shipItem.Components.GetComponent<ShipItemComponent>();

            var player = CreateEntity.Empty(contexts)
                .AddId(_identifier.Next())
                .AddPlayerNetworkId(playerNetworkId)
                .AddViewPath(shipComponent.PrefabName)
                .With(x => x.isPlayer = true)
                .With(x => x.isShip = true)
                .With(x=> x.isNetworkTransform = true)

                .AddGlobalPosition(at)
                .AddCurrentSpeedModifier(0)
                .AddCurrentRotationY(0)
                .AddTargetRotation(0)
                .AddCurrentMoveSpeed(0)
                .AddVelocity(Vector2.zero)
                .With(x => x.isMoving = true)

                .AddVelocityAgility(1.5f)   //0.5 - more heavy, 3.5 - more lighter
                .AddRotationSpeed(250f)
                .AddMaxMoveSpeed(15f)
                .AddMovingAcceleration(3f)
                .AddPhysicsRadius(5)
                .AddMass(100)
                ;

            return player;
        }
    }
}
