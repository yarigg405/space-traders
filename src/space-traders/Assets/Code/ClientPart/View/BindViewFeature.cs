using Assets.Code.ClientPart.View.Systems;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.ClientPart.View
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
