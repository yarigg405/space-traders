using Assets.Code.ClientPart.Gameplay.Features.Destruct;
using Assets.Code.ClientPart.Gameplay.Features.InputInteraction;
using Assets.Code.ClientPart.Gameplay.Features.Movement;
using Assets.Code.ClientPart.Gameplay.Features.Player;
using Assets.Code.ClientPart.View;
using Assets.Code.Infrastructure.Systems;


namespace Assets.Code.ClientPart.Gameplay.Features
{
    public sealed class ClientGameFeature : Feature
    {
        public ClientGameFeature(ISystemFactory systems)
        {
            Add(systems.Create<BindViewFeature>());
            Add(systems.Create<InputClientFeature>());

            Add(systems.Create<PlayerFeature>());

            Add(systems.Create<MovementClientFeature>());

            Add(systems.Create<ProcessDestructedClientFeature>());
        }
    }
}
