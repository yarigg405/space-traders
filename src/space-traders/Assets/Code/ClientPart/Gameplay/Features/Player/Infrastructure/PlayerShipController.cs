using Assets.Code.ClientPart.Networking;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure
{
    public sealed class PlayerShipController
    {
        internal void SetSpeedModifier(float value)
        {
            ClientMessenger.SendSpeedModifierToServer(value);
        }
    }
}
