using Assets.Code.ClientPart.Gameplay.Features.InputInteraction;
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

        internal void SetSpeedModifier(float value)
        {
            _clientMessenger.SendSpeedModifierToServer(value);
        }

        internal void SetKeepDistance(ClickableEntity currentSelected, Vector2 minMaxDistance)
        {
            _clientMessenger.SendKeepDistance(currentSelected.Entity.Id, minMaxDistance);
        }

        internal void SetOrbit(ClickableEntity currentSelected, float orbitRadius)
        {
            _clientMessenger.SendSetOrbit(currentSelected.Entity.Id, orbitRadius);
        }

        internal void SetWarpTo(double2 coordinates)
        {
            _clientMessenger.SendSetWarpTo(coordinates);
        }
    }
}
