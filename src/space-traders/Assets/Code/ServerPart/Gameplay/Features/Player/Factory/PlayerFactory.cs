using Assets.Code.Common;
using Assets.Code.Common.Extensions;
using Assets.Code.Infrastructure.Identifiers;
using Unity.Mathematics;


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
                .AddPlayerNetworkId(playerNetworkId)
                .AddGlobalPosition(at)
                .AddViewPath("Prefabs/PlayerShip")

                .With(x => x.isPlayer = true);

            return player;
        }
    }
}
