using Assets.Code.ClientPart.Gameplay.Features.Movement.Systems;
using Assets.Code.ClientPart.Gameplay.Features.Player.Systems;
using Assets.Code.Infrastructure.Systems;
using Assets.Code.ServerPart.Gameplay.Features.SkyboxObjects.Systems;


namespace Assets.Code.ClientPart.Gameplay.Features.Movement
{
    public sealed class PresentationMovementFeature : Feature
    {
        public PresentationMovementFeature(ISystemFactory systems)
        {
            Add(systems.Create<PlayerPreviousFramePositionSystem>());

            Add(systems.Create<InterpolateNetworkEntitiesSystem>());
            Add(systems.Create<UpdateLocalPositionSystem>());
            Add(systems.Create<UpdateViewModelSystem>());

            Add(systems.Create<UpdateSkyboxSpaceStateSystem>());
            Add(systems.Create<UpdateSkyboxLocalPositionSystem>());

            Add(systems.Create<UpdateTransformRotationSystem>());
            Add(systems.Create<UpdateTransformPositionSystem>());
        }
    }
}
