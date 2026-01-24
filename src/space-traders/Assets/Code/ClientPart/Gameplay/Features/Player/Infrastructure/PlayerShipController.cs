using Assets.Code.ClientPart.Gameplay.Features.InputInteraction;
using Assets.Code.ClientPart.Networking;
using Unity.Mathematics;
using UnityEngine;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerShipController
    {
        internal void SetSpeedModifier(float value)
        {
            ClientMessenger.SendSpeedModifierToServer(value);
        }

        internal void SetKeepDistance(ClickableEntity currentSelected, Vector2 minMaxDistance)
        {
            
        }

        internal void SetOrbit(ClickableEntity currentSelected, float orbitRadius)
        {
            
        }

        internal void SetWarpTo(double2 coordinates)
        {
            
        }
    }
}
