using Assets.Code.Gameplay.Features.Lifetime.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.Gameplay.Features.Lifetime
{
    public sealed class DeathFeature :Feature
    {
        public DeathFeature(ISystemFactory systems)
        {
            Add(systems.Create<MarkDeadSystem>());
        }
    }
}
