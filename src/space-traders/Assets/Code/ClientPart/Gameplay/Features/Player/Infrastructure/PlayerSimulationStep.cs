using Assets.Code.ClientPart.Gameplay.Features.InputInteraction;
using Assets.Code.Common.Extensions;


namespace Assets.Code.ClientPart.Gameplay.Features.Player.Infrastructure
{
    public static class PlayerSimulationStep
    {
        public static void Apply(GameEntity player)
        {
            PlayerInputIntegration.Apply(player);
            MovementFormulas.UpdateMoveSpeed(player);
            if (player.isMoving)
            {
                MovementFormulas.Rotate(player);
                MovementFormulas.Move(player);
            }
        }
    }
}
