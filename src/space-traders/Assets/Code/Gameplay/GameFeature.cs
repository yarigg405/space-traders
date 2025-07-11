using Assets.Code.Common.Destruct;
using Assets.Code.Gameplay.Features.DamageApplication;
using Assets.Code.Gameplay.Features.Movement;
using Assets.Code.Gameplay.Features.Player;
using Assets.Code.Gameplay.Features.TargetCollection;
using Assets.Code.Gameplay.InputInteraction;
using Assets.Code.Infrastructure.Systems;
using Assets.Code.View;


namespace Assets.Code.Gameplay
{
    internal sealed class GameFeature : Feature
    {
        public GameFeature(ISystemFactory systems)
        {
            Add(systems.Create<BindViewFeature>());
            Add(systems.Create<InputFeature>());
            Add(systems.Create<PlayerFeature>());
            Add(systems.Create<MovementFeature>());
            Add(systems.Create<CollectTargetsFeature>());
            Add(systems.Create<DamageApplicationFeature>());

            Add(systems.Create<ProcessDestructedFeature>());
        }
    }
}
