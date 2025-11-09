using Assets.Code.ClientPart.Gameplay.Features.Destruct;
using Assets.Code.ClientPart.View;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.ClientPart.Gameplay.Features
{
    public sealed class ClientGameFeature : Feature
    {
        public ClientGameFeature(ISystemFactory systems)
        {
            Add(systems.Create<BindViewFeature>());

            Add(systems.Create<ProcessDestructedClientFeature>());
        }
    }
}
