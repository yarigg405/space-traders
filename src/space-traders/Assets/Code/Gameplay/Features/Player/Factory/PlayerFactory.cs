using Assets.Code.Common.Entity;
using Assets.Code.Infrastructure.Identifiers;
using Code.Common.Extensions;
using Unity.Mathematics;


namespace Assets.Code.Gameplay.Features.Player.Factory
{
    public sealed class PlayerFactory
    {
        private readonly IIdentifierService _identifier;

        public PlayerFactory(IIdentifierService identifier)
        {
            _identifier = identifier;
        }

        public GameEntity CreatePlayer(ushort playerNetworkId, string sceneForPlayer,  double2 at, Contexts contexts)
        {
            var player = CreateEntity.Empty(contexts)
                .AddId(_identifier.Next())
                .AddCurrentScene(sceneForPlayer)
                .AddPlayerNetworkId(playerNetworkId)
                .AddGlobalPosition(at)
                .AddViewPath("Prefabs/PlayerShip")

                .With(x => x.isPlayer = true);

            return player;
        }
    }
}
