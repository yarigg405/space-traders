using Assets.Code.Infrastructure.Systems;
using Assets.Code.View.Systems;


namespace Assets.Code.View
{
    public sealed class BindViewFeature : Feature
    {
        public BindViewFeature(ISystemFactory systems)
        {
            Add(systems.Create<BindEntityViewFromPathSystem>());
            Add(systems.Create<BindEntityViewFromPrefabSystem>());
        }
    }
}
