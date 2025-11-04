using Assets.Code.Infrastructure.Systems;
using Assets.Code.View;


namespace Assets.Code.Gameplay.Features
{
    public sealed class ClientGameFeature : Feature
    {
        public ClientGameFeature(ISystemFactory systems)
        {
            Add(systems.Create<BindViewFeature>());
        }
    }
}
