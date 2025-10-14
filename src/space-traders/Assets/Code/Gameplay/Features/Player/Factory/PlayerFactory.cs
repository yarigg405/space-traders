using Assets.Code.Common.Entity;
using Assets.Code.Infrastructure.Identifiers;
using Code.Common.Extensions;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.Gameplay.Features.Player.Factory
{
    public sealed class PlayerFactory
    {
        private readonly IIdentifierService _identifier;

        public PlayerFactory(IIdentifierService identifier)
        {
            _identifier = identifier;
        }

        public GameEntity CreatePlayer(double2 at)
        {
            var player = CreateEntity.Empty()
                .AddId(_identifier.Next())
                .AddGlobalPosition(at)
                .AddLocalPosition(Vector3.zero)

                .AddViewPath("Prefabs/PlayerShip")

                .With(x => x.isPlayer = true);

            return player;
        }
    }
}
