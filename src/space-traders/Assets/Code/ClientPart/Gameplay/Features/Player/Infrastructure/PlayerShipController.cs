using Assets.Code.ClientPart.Gameplay.Features.Navigation;
using Assets.Code.ClientPart.Networking;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerShipController
    {
        private readonly ClientMessenger _clientMessenger;

        public PlayerShipController(ClientMessenger clientMessenger)
        {
            _clientMessenger = clientMessenger;
        }

        internal void SetKeepDistance(GameEntity target, Vector2 minMaxDistance)
        {
            _clientMessenger.SendKeepDistance(target.Id, minMaxDistance);
        }

        internal void SetOrbit(GameEntity target, float orbitRadius)
        {
            _clientMessenger.SendSetOrbit(target.Id, orbitRadius);
        }

        internal void SetWarpTo(double2 coordinates)
        {
            _clientMessenger.SendSetWarpTo(coordinates);
        }

        internal bool SetWarpToEntity(GameEntity target)
        {
            if (!target.TryGetCoordinate(out var coordinates))
                return false;

            _clientMessenger.SendSetWarpTo(coordinates);
            return true;
        }
    }
}
