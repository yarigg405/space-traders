using Assets.Code.Common;
using Unity.Mathematics;


namespace Assets.Code.ClientPart.Gameplay.Features.InputInteraction
{
    public static class PlayerInputIntegration
    {
        public static void Apply(GameEntity player)
        {
            var input = player.MoveInput;
            if (input.sqrMagnitude < 0.001f) return;

            player.ReplaceTargetRotation(player.TargetRotation + input.x * 30f
                * GameConstants.FIXED_DELTA_TIME);
            var speed = math.clamp(player.CurrentSpeedModifier + input.y * 1f
                * GameConstants.FIXED_DELTA_TIME, 0f, 1f);
            player.ReplaceCurrentSpeedModifier(speed);
        }
    }
}
